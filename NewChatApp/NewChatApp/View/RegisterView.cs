using NewChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace NewChatApp.View
{
    internal class RegisterView
    {
        public void RegisterViewer()
        {
            Console.WriteLine();
            Console.WriteLine("You are in Registration View!");
            Console.WriteLine("Try register yourself or pick ./help");
            string[] line;
            do
            {
                Console.WriteLine("Enter some comand to do operation or ./back to leave");
                line = Console.ReadLine().Split().ToArray();
                if (line[0] == "/help")
                {
                    PrintAllComands();
                }
                else if (line[0] == "/register")
                {
                    RegisterRealize(line[1], line[2], line[3]);
                }
            } while (line[0] == "/back" || line[0] == "/BACK");
            return;
        }

        private void RegisterRealize(string email, string username, string pass)//TODO
        {
            Registration newRegistration = new Registration(username,pass,email,DateTime.Now.ToString());
            if (newRegistration.IsRegistered)
            {
                UserView userView = new UserView(username, email, pass);
                userView.UserViewer();
            }
        }

        private void PrintAllComands()
        {
            Console.WriteLine("./register <email> <username> <password>");
            Console.WriteLine("./back");
        }
    }
}
