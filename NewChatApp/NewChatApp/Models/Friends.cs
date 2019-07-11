using System;
using System.Collections.Generic;
using System.Text;

namespace NewChatApp.Models
{
    internal class Friends
    {
        public List<string> Invitations { get; set; }
        public List<string> FriendsUsernames { get; private set; }
        public string RemoveFriendUsername { get; private set; }
        public void CheckInvitationList() { }
    }
}
