using System;

namespace chatapplication
{
    public class User
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public Friends Friends { get; private set; }
        public User(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));


            UserName = userName;
            Password = password;
            Friends = new Friends(UserName);
        }
    }
}
