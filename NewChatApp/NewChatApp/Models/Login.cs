using System;
using System.Collections.Generic;
using System.Text;

namespace NewChatApp.Models
{
    internal class Login
    {
        private char[] Chars = new char[] { '/', '\\', '#', '&', '\'', '\"', '*' };

        public string Username { get; set; }
        private string username
        {
            get
            {
                return this.Username;
            }
            set
            {
                if (ValidInput(value))
                    if (ValidUsername(value))
                    {
                        this.Username = value;
                    }
                    else
                    {
                        throw new Exception("Please check your Username");
                    }
                else
                {
                    throw new Exception("Please check your input into username box");
                }
            }
        }

        private bool ValidUsername(string username)//TODO
        {
            throw new NotImplementedException();
        }

        private bool ValidInput(string line)
        {
            if (line.Contains(this.Chars.ToString()))
                return true;
            return false;
        }

        public string Password { get; set; }
        private string password
        {
            get
            {
                return this.Password;
            }
            set
            {
                if (ValidInput(value))
                    if (ValidPassword(value))
                    {
                        this.Password = value;
                    }
                    else
                    {
                        throw new Exception("Please check your Password");
                    }
                else
                {
                    throw new Exception("Please check your input into password box");
                }
            }
        }

        private bool ValidPassword(string value)//TODO
        {
            throw new NotImplementedException();
        }

        public string LastLoginInformation { get; private set; }

        public Login(string username,string password,string lastlogininformation)
        {
            this.username = username;
            this.password = password;
            this.LastLoginInformation = lastlogininformation;
            MakeLogin();
        }
        public bool LoginSuccesfull = false;
        private void MakeLogin()//TODO
        {
            throw new NotImplementedException();
            this.LoginSuccesfull = true;
        }
    }
}
