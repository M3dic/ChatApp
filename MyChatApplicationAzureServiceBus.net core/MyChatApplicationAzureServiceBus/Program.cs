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
    /// @Author : Loic Sterckx
    /// @Date : 30 October 2016
    /// <summary>
    /// Class that contains the main method of the software
    /// </summary>
    class Program
    {
        private static string UserName;
        private static string Password;
        private static string Email;

        static void Main(string[] args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            //Initiate the topic if not exist
            Helper.CreateTopic();

            //get the current user information
            JoinChatParticipant();

        }

        private static void JoinChatParticipant()
        {
            Console.WriteLine("Login or Register");
            string option = Console.ReadLine();
            if (option == "Login")
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
                    MainMenu();
            }
            else if (option == "Register")
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
                    MainMenu();
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
            User user = new User(UserName, Password);
            Console.WriteLine("Hello " + UserName + ", which mode do you want to use?");
            Console.WriteLine("1. Receive My Messages");
            Console.WriteLine("2. Invite friends");
            Console.WriteLine("3. Invitations");
            Console.WriteLine("4. Send Messages");
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
            FriendsConstructorBaseInput invitationlist = new FriendsConstructorBaseInput();
            invitationlist.GetInvitaions(UserName);
        }
        /// <summary>
        /// Display inviting friends
        /// </summary>
        private static void DisplayInviteFriends()
        {
            Console.WriteLine("Which friends do you want to invite? (Press 1 and enter to return to main menu)");
            foreach (SubscriptionDescription subscription in Helper.GetSubscriptionsNames())
            {
                Console.WriteLine("\t" + subscription.SubscriptionName);
            }
            List<string> friendsnames = Console.ReadLine().ToLowerInvariant().Split().ToList();
            if (friendsnames[0] == "1")
            {
                MainMenu();
                return;
            }
            FriendsConstructorBaseInput friendsConstructor = new FriendsConstructorBaseInput();
            foreach (var friendname in friendsnames)
            {
                friendsConstructor.InviteFriend(UserName, friendname);
            }
        }

        /// <summary>
        /// Method to display the receiver menu of the software
        /// </summary>
        private static void DisplayReceiverMenu()
        {
            Console.WriteLine("New Messages will be received here. (Press 1 and enter to return to main menu)");
            Helper.ReceiveMessageSubscription(UserName);
            string result = Console.ReadLine();
            if (result == "1")
            {
                MainMenu();
                return;
            }
        }

        /// <summary>
        /// Method to display the send menu of the software
        /// </summary>
        private static void DisplaySenderMenu()
        {
            Console.WriteLine("To who will you send a new message? (press 1 and enter to go to home menu)");
            FriendsConstructorBaseInput friendsConstructor = new FriendsConstructorBaseInput();
            //Get it hash set because of usefull check operation if many invitaions have sent
            HashSet<string> peoples = friendsConstructor.GetFriendsNames(UserName);

            foreach (var item in peoples)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("All");
            string toUserName = Console.ReadLine();
            //check if we have to exit
            if (toUserName == "1")
            {
                MainMenu();
                return;
            }
            //check if the sender exist            
            if (toUserName.ToLowerInvariant() != "all" && !Helper.IsSubscriptionExist(toUserName))////////
            {
                Console.WriteLine("Your user doesn't exist, please choose another one");
                DisplaySenderMenu();
                return;
            }

            if (toUserName == "all")
            {
                ChatPartisipants chat = new ChatPartisipants("Fast chat", peoples.ToList());
                Console.WriteLine("Type your message for " + string.Join(", ", toUserName.ToList()));
                string message = Console.ReadLine();
                //check if we still don't have to exit
                if (message == "1")
                {
                    MainMenu();
                    return;
                }
                chat.SendMessage(message, UserName);
            }
            else
            {
                ChatPartisipants chat = new ChatPartisipants("Fast chat", toUserName.Split().ToList());
                Console.WriteLine("Type your message for " + string.Join(", ", toUserName.ToList()));
                string message = Console.ReadLine();
                //check if we still don't have to exit
                if (message == "1")
                {
                    MainMenu();
                    return;
                }
                chat.SendMessage(message, UserName);
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
