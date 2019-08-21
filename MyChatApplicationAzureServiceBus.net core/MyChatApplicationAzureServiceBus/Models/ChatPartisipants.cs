using ChatServiceBus;
using System;
using System.Collections;
using System.Collections.Generic;

namespace chatapplication
{
    public class ChatPartisipants
    {
        public HashSet<string> ChatPartisipantsNames { get; private set; }
        public string Message { get; set; }
        public ChatPartisipants(HashSet<string> username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            ChatPartisipantsNames = username;
        }
        public void SendMessage(string message, string fromuser)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));
            Message = message;
            foreach (var toUserName in ChatPartisipantsNames)
            {
                AzureServiceBusHelper.SendMessageTopic(toUserName.ToString(), fromuser, message);
            }
            Console.WriteLine("\n**Message Sent!**");
        }
        public void AddPeopleToChat(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));
            ChatPartisipantsNames.Add(username);
        }

    }
}
