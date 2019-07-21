using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewChatApp.View
{
    internal class UserView
    {
        private string Name { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        public UserView(string name, string email, string pass)
        {
            this.Name = name;
            this.Email = email;
            this.Password = pass;
        }
        public UserView(string name, string pass)
        {
            this.Name = name;
            this.Email = null;
            this.Password = pass;
        }
        public void UserViewer()
        {
            FriendsView friendsView = new FriendsView(this.Name);
            Console.WriteLine();
            Console.WriteLine("You are in User View!");
            Console.WriteLine("Should pick a comand or ./help");
            string[] line;
            do
            {
                Console.WriteLine("Enter some comand to do operation or ./leave to leave: ");
                line = Console.ReadLine().Split().ToArray();
                if (line[0] == "/help")
                {
                    PrintAllComands();
                }
                else if (line[0] == "/friends")
                {

                    friendsView.ShowList();
                }
                else if (line[0] == "/invite" && line[1] == "friends")
                {
                    friendsView.InviteFriends(line[2]);
                }
                else if (line[0] == "/invitations")
                {
                    friendsView.CheckInvitationList();
                }
                else if (line[0] == "/chat")
                {
                    Chat1_1 chat1_1 = new Chat1_1(line[1]);
                    chat1_1.OpenChat();
                }
                else if (line[0] == "/groupChat")
                {
                    //GROUPCHAT WITH FRIEND
                }else if (line[0] == "/remove")
                {
                    string name = line[1];
                    friendsView.RemoveFrindFromList(name);
                }
                else if (line[0] == "/settings")
                {
                    //OPEN SETTINGS
                }
            } while (line[0] == "/leave" || line[0] == "/LEAVE");
            return;
        }

        private void PrintAllComands()
        {
            Console.WriteLine("./leave");
            Console.WriteLine("./friends");
            Console.WriteLine("./invite friends <username>");
            Console.WriteLine("./invitations");
            Console.WriteLine("./chat <friend username>");
            Console.WriteLine("./groupChat <friend username> <friend username> ...");
            Console.WriteLine("./remove <friend username>");
            Console.WriteLine("./settings");
        }
    }
}
