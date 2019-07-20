using System;
using System.Collections.Generic;
using NewChatApp.Models;
using System.Text;

namespace NewChatApp.View
{
    internal class Chat1_1
    {
        private Chat1_to_1 chat1_To_1;
        public Chat1_1(string name)
        {
            this.chat1_To_1 = new Chat1_to_1(name);
        }

        internal void OpenChat()
        {
            //string message = null;
            Console.WriteLine($"--CHAT WITH {this.chat1_To_1.UsernameToContact}--");
            //open connection
            //chat
            //read
            //close connection
        }
    }
}
