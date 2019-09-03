using chatapplication;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChatApplicationAzureServiceBus.Constructor;

namespace ChatServiceBus
{
    class Program
    {
        private static string UserName;
        private static string Password;
        private static string Email;
        private static User user;

        static void Main(string[] args)
        {
            //get the current user information
            JoinChatParticipant();
        }

        private static void JoinChatParticipant()
        {
            Console.WriteLine("Login or Register (press 0 and enter to leave project)");
            string option = Console.ReadLine();
            if (option == "0")
                Environment.Exit(0);
            else if (option.ToLowerInvariant() == "login")
            {
                //get the current user information
                Console.WriteLine("Welcome on ChatServiceBus, please enter your user details:");

                Console.Write("Username: ");
                UserName = Console.ReadLine();
                Console.Write("Password: ");
                Password = Console.ReadLine();
                Login login = new Login(UserName, Password);
                if (!login.isLoged)
                    JoinChatParticipant();
                //show the menu
                else
                {
                    user = new User(UserName, Password);
                    MainMenu();
                }
                JoinChatParticipant();
            }
            else if (option.ToLowerInvariant() == "register")
            {
                Console.WriteLine("Welcome on ChatServiceBus, please register yourself:");

                Console.Write("Username: ");
                UserName = Console.ReadLine();
                Console.Write("Password: ");
                Password = Console.ReadLine();
                Console.Write("Email: ");
                Email = Console.ReadLine();

                Registration registration = new Registration(UserName, Password, Email);
                if (!registration.isRegistered)
                    JoinChatParticipant();
                //show the menu
                else
                {
                    user = new User(UserName, Password);
                    MainMenu();
                }
                JoinChatParticipant();
            }
            else
            {
                JoinChatParticipant();
                return;
            }

        }

        /// <summary>
        /// Method to display the main menu of the software
        /// </summary>
        private static void MainMenu()
        {
            Console.WriteLine("Hello " + UserName + ", which mode do you want to use?");
            Console.WriteLine("1. Receive my messages");
            Console.WriteLine("2. Invite friends");
            Console.WriteLine("3. Invitations");
            Console.WriteLine("4. Send messages");
            Console.WriteLine("5. Exit");
            string result = Console.ReadLine();
            switch (result)
            {
                case "1":
                    DisplayReceiverMenu();
                    break;
                case "2":
                    DisplayInviteFriends();
                    break;
                case "3":
                    DisplayInvitations();
                    break;
                case "4":
                    DisplaySenderMenu();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    MainMenu();
                    break;
            }
        }
        /// <summary>
        /// Display all invitations to current user
        /// </summary>
        private static void DisplayInvitations()
        {
            Console.WriteLine("Invitation list. (Press 1 and enter to return to main menu)");
            user.Friends.LoadFriends();
            MainMenu();
        }
        /// <summary>
        /// Display inviting friends
        /// </summary>
        private static void DisplayInviteFriends()
        {
            Console.WriteLine("Which friends do you want to invite? (Press 1 and enter to return to main menu)");
            foreach (string subscription in FriendsConstructorBaseInput.GetSubscriptionsNames(user.UserName))
            {
                Console.WriteLine("\t" + subscription);
            }
            Console.Write("Write names: ");
            HashSet<string> friendsnames = Console.ReadLine().ToLowerInvariant().Split().ToHashSet();
            if (friendsnames.First() == "1")
            {
                MainMenu();
            }
            else
            {
                user.Friends.AddNewFriends(friendsnames);
                MainMenu();
            }
        }

        /// <summary>
        /// Method to display the receiver menu of the software
        /// </summary>
        private static void DisplayReceiverMenu()
        {
            Console.WriteLine("New Messages will be received here.");
            foreach (var topic in AzureServiceBusHelper.TakeAllTopicsForUser(user.UserName))
            {
                AzureServiceBusHelper.ReceiveMessageSubscription(topic, user.UserName);
            }
            Console.ReadLine();
            MainMenu();
        }

        /// <summary>
        /// Method to display the send menu of the software
        /// </summary>
        private static void DisplaySenderMenu()
        {
            Console.WriteLine("To who will you send a new message? (press 1 and enter to go to home menu)");

            Console.WriteLine(string.Join("  \n", user.Friends.GetFriendsUsernames()));
            Console.WriteLine("All");
            string toUserName = Console.ReadLine();

            //check if we have to exit
            if (toUserName == "1")
            {
                MainMenu();
                return;
            }
            List<string> topicpats = user.Friends.GetAllTopics();
            if (toUserName == "all")
            {
                ChatPartisipants chat = new ChatPartisipants(user.Friends.GetFriendsUsernames().ToHashSet());
                Console.WriteLine("Type your message for " + string.Join(", ", user.Friends.GetFriendsUsernames().ToHashSet()));
                string message = Console.ReadLine();
                //check if we still don't have to exit
                if (message == "1")
                {
                    MainMenu();
                }
                else
                chat.SendMessage(topicpats, message, user.UserName);
            }
            else
            {
                HashSet<string> users = new HashSet<string>();
                foreach (var item in toUserName.Split(' ').ToHashSet())
                    if (user.Friends.GetFriendsUsernames().Contains(item))
                        users.Add(item);

                ChatPartisipants chat = new ChatPartisipants(user.UserName,users);
                Console.WriteLine("Type your message for " + string.Join(", ", users));
                string message = Console.ReadLine();
                //check if we still don't have to exit
                if (message == "1")
                {
                    MainMenu();
                }
                else
                chat.SendMessage(topicpats,message, user.UserName);
            }

            //check to send another message or not
            Console.WriteLine("Do you want to send another one? (Y/N)");
            switch (Console.ReadLine().ToLowerInvariant())
            {
                case "y":
                    DisplaySenderMenu();
                    break;
                case "n":
                    MainMenu();
                    break;
                default:
                    MainMenu();
                    break;
            }
        }


    }
}
