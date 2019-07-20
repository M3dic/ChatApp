using System;
using System.Collections.Generic;
using System.Text;

namespace NewChatApp.Models
{
    internal class MultipleChat
    {
        public List<string> FriendsToChat { get; set; }
        private List<string> friendstochat
        {
            get
            {
                return this.FriendsToChat;
            }
            set
            {
                if (value.Count != 0 && value.Count != 1)
                {
                    this.FriendsToChat = value;
                }
                else
                {
                    throw new Exception("Please ensure you have picked more than one friend for group chat");
                }
            }
        }
        public string Message { get; private set; }
        private string message
        {
            get
            {
                return this.Message;
            }
            set
            {
                if (this.FriendsToChat != null)
                {
                    this.Message = value;
                    SendMessageToGroup(this.Message);
                }
                else
                {
                    throw new Exception("Please check your group");
                }
            }
        }

        private void SendMessageToGroup(string message)//TODO
        {
            throw new NotImplementedException();
        }

        public MultipleChat(List<string> friends)
        {
            this.friendstochat = friends;
        }

        public void SendMessage(string message)
        {
            if (checkmessage(message))
            {
                this.message = message;
            }
            else
            {
                throw new Exception("Please check your message for symbols like \"#,%,*,~,\',\",\\\"");
            }
        }
        private char[] Chars = new char[] { '/', '\\', '#', '&', '\'', '\"', '*' };
        private bool checkmessage(string message)
        {
            if (message.Contains(this.Chars.ToString()))
                return false;
            return true;
        }
    }
}
