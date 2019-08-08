using System;
using System.Collections.Generic;

namespace chatapplication
{
    public class Friends
    {
        private readonly List<string> FriendsUsernames;

        public List<string> GetFriendsUsernames()
        {
            return FriendsUsernames;
        }

        public void SetFriendsUsernames(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("message", nameof(username));

            FriendsUsernames.Add(username);
        }
        public Friends(string MyUsername)
        {
            if (string.IsNullOrWhiteSpace(MyUsername))
                throw new ArgumentException("message", nameof(MyUsername));

            FriendsUsernames = new List<string>();
            LoadFriends(MyUsername);
        }
        private void LoadFriends(string myname)
        {
            if (myname is null)
                throw new ArgumentNullException(nameof(myname));

            throw new NotImplementedException();
        }
        public void RemoveFriend(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("message", nameof(name));
            FriendsUsernames.Remove(name);
        }
        public void AddNewFriends(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("message", nameof(name));
            FriendsUsernames.Add(name);
        }
    }
}
