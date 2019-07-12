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

        private void SendMessage()//TODO
        {
            throw new NotImplementedException();
        }

        private readonly char[] Chars = new char[] { '/', '\\', '#', '&', '\'', '\"', '*' };
        private bool ValidMessage(string message)
        {
            if (message.Contains(this.Chars.ToString()))
                return true;
            return false;
        }

        public Chat1_to_1(string usernametochat,string message)
        {
            this.UsernameToContact = usernametochat;
            this.message = message;
        }
    }
}
