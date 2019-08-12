using System;
using MyChatApplicationAzureServiceBus.Constructor;

namespace chatapplication
{
    public class Login
    {
        public bool isLoged;

        public Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("message", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("message", nameof(password));

            Username = username;
            Password = password;
            LoginDataBaseInput loginDatabase = new LoginDataBaseInput(this);
            if (loginDatabase.LoginUser())
            {
                LoginDetails = new User(Username, Password);
                Console.WriteLine("Successfully login");
                isLoged = true;
            }
            else
                isLoged = false;
        }

        public User LoginDetails { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
    }
}
