using System;
using System.Collections.Generic;
using System.Text;

namespace NewChatApp.Models
{
    internal class Friends
    {
        public List<string> Invitations { get; private set; }
        public List<string> FriendsUsernames { get; private set; }
        public void CheckInvitationList()//TODO
        {

        }
        public void ShowFriendList()//TODO
        {

        }
        public void RemoveFriendFromList(string friendName)
        {
            if (this.FriendsUsernames.Contains(friendName))
            {
                this.FriendsUsernames.Remove(friendName);
            }
            else
            {
                throw new InvalidOperationException("You do not have such friend");
            }
        }
        
    }
}
