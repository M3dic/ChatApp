﻿using NewChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewChatApp.View
{
    internal class RegisterView
    {
        public void RegisterViewer()
        {
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
                else if (line[0] == "/Register")
                {
                    RegisterRealize(line[1], line[2], line[3]);
                }
            } while (line[0] == "/end" || line[0] == "/END");
            return;
        }

        private void RegisterRealize(string email, string username, string pass)//TODO
        {
            Registration newRegistration = new Registration(username,pass,email,DateTime.Now.ToString());
            if (newRegistration.IsRegistered)
            {
                //Open User view
            }
        }

        private void PrintAllComands()
        {
            Console.WriteLine("./Register <email> <username> <password>");
            Console.WriteLine("./end");
        }
    }
}
