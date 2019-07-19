using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewChatApp.View
{
    public class UserView
    {
        private string Name { get; set; }
        private string Email { get; set; }
        public void UserViewer()
        {
            Console.WriteLine();
            Console.WriteLine("You are in User View!");
            Console.WriteLine("Should pick a comand or ./help");
            string[] line;
            do
            {
                Console.WriteLine("Enter some comand to do operation or ./end to leave: ");
                line = Console.ReadLine().Split().ToArray();
                if (line[0] == "/help")
                {
                    PrintAllComands();
                }
                else if (line[0] == "/Friends")
                {
                    //SHOW FRIENDS LIST
                }
                else if (line[0] == "/Invite" && line[1] == "friends")
                {
                    //INVITE FRIEND
                }
                else if (line[0] == "/Invitations")
                {
                    //SHOW INVITATIONS
                }
                else if (line[0] == "/Chat")
                {
                    //CHAT WITH FRIEND
                }
                else if (line[0] == "/GroupChat")
                {
                    //GROUPCHAT WITH FRIEND
                }
                else if (line[0] == "/Settings")
                {
                    //OPEN SETTINGS
                }
            } while (line[0] == "/end" || line[0] == "/END");
            return;
        }

        private void PrintAllComands()
        {
            Console.WriteLine("./end");
            Console.WriteLine("./Friends");
            Console.WriteLine("./Invite friends <username>");
            Console.WriteLine("./Invitations");
            Console.WriteLine("./Chat <friend username>");
            Console.WriteLine("./GroupChat <friend username> <friend username> ...");
            Console.WriteLine("./Settings");
        }
    }
}
