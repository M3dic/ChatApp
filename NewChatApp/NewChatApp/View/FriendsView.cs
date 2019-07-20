using NewChatApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewChatApp.View
{
    internal class FriendsView
    {
        private Friends friendList { get; set; }
        private string username { get; set; }
        public string FriendName { get; set; }
        private string friendname
        {
            get
            {
                return this.FriendName;
            }
            set
            {
                if (ValidInput(value))
                {
                    this.FriendName = value;
                }
            }
        }
        private char[] Chars = new char[] { '/', '\\', '#', '&', '\'', '\"', '*' };

        private bool ValidInput(string value)
        {
            if (value.Contains(this.Chars.ToString()))
                return false;
            return true;
        }

        public FriendsView(string name)
        {
            this.username = name;
            this.friendList = new Friends();
        }
        public void ShowList()
        {
            this.friendList.ShowFriendList(this.username);
        }
        public void InviteFriends(string Fname)
        {
            this.friendname = Fname;
            this.friendList.InviteFriend(this.FriendName);
        }
        public void CheckInvitationList()
        {
            Console.WriteLine("--INVITATION LIST--");
            foreach (var item in this.friendList.Invitations)
            {
                Console.WriteLine("New Invitation from: " + item);
                Console.Write("Accept [y/n]: ");
                string acceptance = Console.ReadLine();
                if (acceptance == "y")
                {
                    this.friendList.FriendsUsernames.Add(item);
                    this.friendList.Invitations.Remove(item);
                    //UPLOAD DB
                }
                else
                {
                    this.friendList.Invitations.Remove(item);
                    //UPLOAD DB
                }
            }
            Console.WriteLine();
        }
        public void RemoveFrindFromList(string n)
        {
            this.friendList.RemoveFriendFromList(n);
        }
    }
}
