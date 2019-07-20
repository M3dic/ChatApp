using System;
using System.Collections.Generic;
using System.Text;

namespace NewChatApp.Models
{
    internal class Chat1_to_1
    {
        public string UsernameToContact { get; private set; }
        
        public string Message { get; set; }
        private string message
        {
            get
            {
                return this.Message;
            }
            set
            {
                if (ValidMessage(value))
                {
                    this.Message = value;
                    if(this.UsernameToContact!=null||this.UsernameToContact!=" ")
                    {
                        SendMessage();
                    }
                }
                else
                {
                    throw new Exception("Please check your message for symbols like \"#,%,*,~,\',\",\\\"");
                }
            }
        }

        public void SendMessage()//TODO
        {
            //send this.Message to Reciever
            throw new NotImplementedException();
        }

        private readonly char[] Chars = new char[] { '/', '\\', '#', '&', '\'', '\"', '*' };
        private bool ValidMessage(string message)
        {
            if (message.Contains(this.Chars.ToString()))
                return false;
            return true;
        }

        public Chat1_to_1(string usernametochat)
        {
            this.UsernameToContact = usernametochat;
        }
    }
}
