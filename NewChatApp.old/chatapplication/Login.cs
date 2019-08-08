using System;

namespace chatapplication
{
    public class Login
    {
        public Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("message", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("message", nameof(password));

            Username = username;
            Password = password;
            LoginDetails = new User(Username, Password);
        }
        public User LoginDetails { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
    }
}
