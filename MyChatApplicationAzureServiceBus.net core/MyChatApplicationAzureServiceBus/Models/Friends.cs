using ChatServiceBus;
using MyChatApplicationAzureServiceBus.Constructor;
using System;
using System.Collections.Generic;

namespace chatapplication
{
    public class Friends
    {
        private HashSet<string> FriendsUsernames;
        private List<string> TopicsPaths;
        private string MyName { get; set; } 

        public HashSet<string> GetFriendsUsernames()
        {
            return FriendsUsernames;
        }

        public void SetFriendsUsernames(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            FriendsUsernames.Add(username);
        }
        public Friends(string MyUsername)
        {
            if (string.IsNullOrWhiteSpace(MyUsername))
                throw new ArgumentNullException(nameof(MyUsername));

            MyName = MyUsername;
            FriendsConstructorBaseInput friendsConstructor = new FriendsConstructorBaseInput();
            //Get it hash set because of usefull check operation if many invitaions have sent
            HashSet<string> peoples = friendsConstructor.GetFriendsNames(MyName);
            FriendsUsernames = peoples;
            TopicsPaths = AzureServiceBusHelper.TakeAllTopicsForUser(MyUsername);
        }
        public void AddacceptedFriend(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.FriendsUsernames.Add(name);
        }
        public void LoadFriends()
        {

            FriendsConstructorBaseInput invitationlist = new FriendsConstructorBaseInput();
            invitationlist.GetInvitaions(this,MyName);
        }
        public void RemoveFriend(string name)//TODO
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            FriendsUsernames.Remove(name);
        }
        public void AddNewFriends(HashSet<string> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            FriendsConstructorBaseInput friendsConstructor = new FriendsConstructorBaseInput();
            foreach (var friendname in list)
            {
                friendsConstructor.InviteFriend(MyName, friendname);
            }

            foreach (var item in list)
            {
                FriendsUsernames.Add(item);
            }
        }

        public List<string> GetAllTopics()
        {
            return TopicsPaths;
        }
    }
}
