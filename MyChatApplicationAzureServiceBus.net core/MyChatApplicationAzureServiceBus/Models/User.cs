using System;
using System.Collections;
using System.Collections.Generic;

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
        public ChatPartisipants ChatRefferance { get; private set; }
        public void OpenChat(string chatname, List<string> usernames)
        {
            if (usernames is null)
                throw new ArgumentNullException(nameof(usernames));
            ChatRefferance = new ChatPartisipants(chatname,usernames);
        }
    }
}
