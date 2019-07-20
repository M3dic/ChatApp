using System;
using System.Collections.Generic;
using System.Text;

namespace NewChatApp.Models
{
    internal class Friends
    {
        public List<string> Invitations { get; private set; }
        public List<string> FriendsUsernames { get; private set; }
        public void ShowFriendList(string name)//TODO make connection and check all friends
        {
            Console.WriteLine("--FRIENDS--");
            foreach (var item in this.FriendsUsernames)
            {
                Console.WriteLine("Friend: " + item);
            }
            Console.WriteLine();

        }
        public void InviteFriend(string friendsname)
        {
            //invite
        }
        public void RemoveFriendFromList(string friendName)//TODO
        {
            if (this.FriendsUsernames.Contains(friendName))
            {
                this.FriendsUsernames.Remove(friendName);
                //TODO UPLOAD FRINED LIST
            }
            else
            {
                throw new InvalidOperationException("You do not have such friend");
            }
        }
        
    }
}
