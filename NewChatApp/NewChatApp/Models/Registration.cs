using System;
using System.Collections.Generic;
using System.Text;

namespace NewChatApp.Models
{
    internal class Registration
    {
        public string Username { get; private set; }
        private string username
        {
            get
            {
                return this.Username;
            }
            set
            {
                if (IsValidUsername(value))
                {
                    this.Username = value;
                }
                else
                {
                    throw new Exception("Invalid username");
                }
            }
        }

        private bool IsValidUsername(string name)//TODO
        {
            throw new NotImplementedException();
        }

        public string Password { get; private set; }
        private string password
        {
            get
            {
                return this.Password;
            }
            set
            {
                if (value.Length >= 10)
                {
                    this.Password = value;
                }
                else
                {
                    throw new Exception("Please correct your password");
                }
            }
        }
        public string Email { get; private set; }
        private string email
        {
            get
            {
                return this.Email;
            }
            set
            {
                if (EmailExists(value) && EmailNotUse(value))
                {
                    this.Email = value;
                }
                else
                {
                    throw new Exception("Please correct your email");
                }
            }
        }

        private bool EmailNotUse(string email)//TODO
        {
            throw new NotImplementedException();
        }

        private bool EmailExists(string email)//TODO
        {
            throw new NotImplementedException();
        }

        public string DataRegistered { get; private set; }

        public Registration(string name,string pass,string email,string data)
        {
            if (ValidateInformation(name, pass, email, data))
            {
                this.username = name;
                this.password = pass;
                this.email = email;
                this.DataRegistered = data;
                MakeRegistration(this.Username,this.Email,this.Password,this.DataRegistered);
            }
            else
            {
                throw new Exception("Please check the information below");
            }
        }
        public bool IsRegistered = false;
        private void MakeRegistration(string username, string email, string password, string dataRegistered)//TODO MAKE REGISTRATION
        {
            this.IsRegistered = true;
        }

        private char[] Chars = new char[] { '/', '\\', '#', '&', '\'', '\"', '*' };
        private bool ValidateInformation(string name, string pass, string email, string data)
        {
            if (name.Contains(this.Chars.ToString()))
            {
                return false;
            }
            if (pass.Contains(this.Chars.ToString()))
            {
                return false;
            }
            if (email.Contains(this.Chars.ToString()))
            {
                return false;
            }
            if (data.Contains(this.Chars.ToString()))
            {
                return false;
            }
            return true;
        }
    }
}
