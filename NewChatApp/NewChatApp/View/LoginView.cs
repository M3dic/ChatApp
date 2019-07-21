using NewChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewChatApp.View
{
    internal class LoginView
    {
        public void LoginViewer()
        {
            Console.WriteLine();
            Console.WriteLine("You are in Login View!");
            Console.WriteLine("Try login yourself or pick ./help");
            string[] line;
            do
            {
                Console.WriteLine("Enter some comand to do operation or ./end to leave");
                line = Console.ReadLine().Split().ToArray();
                if (line[0] == "/help")
                {
                    PrintAllComands();
                }
                else if (line[0] == "/login")
                {
                    LoginRealize(line[1], line[2]);
                }
            } while (line[0] == "/back" || line[0] == "/BACK");
            return;
        }

        private void LoginRealize(string user, string pass)
        {
            if (user == null || pass == null)
            {
                Console.WriteLine("Please check your input");
                return;
            }
            Login makeLogin = new Login(user, pass, DateTime.Now.ToString());
            if (makeLogin.LoginSuccesfull)
            {
                UserView userView = new UserView(user,pass);
                userView.UserViewer();
            }
        }

        private void PrintAllComands()
        {
            Console.WriteLine("./login <username> <password>");
            Console.WriteLine("./back");
        }
    }
}
