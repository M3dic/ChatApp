using ChatServiceBus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        public void SendMessage(List<string> TopicNames, string message, string fromuser)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));
            Message = message;
            foreach (var toUserName in ChatPartisipantsNames)
            {
                string TopicName = TopicNames.Find(s => s.Split('1').ToList().Contains(toUserName));//>>>>>>>>?????
                AzureServiceBusHelper.SendMessageTopic(TopicName, toUserName.ToString(), fromuser, message);
            }
            Console.WriteLine("\n**Message Sent!**");
        }
       
    }
}
