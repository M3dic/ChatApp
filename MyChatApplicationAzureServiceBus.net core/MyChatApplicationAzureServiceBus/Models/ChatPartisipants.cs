using ChatServiceBus;
using System;
using System.Collections;
using System.Collections.Generic;

namespace chatapplication
{
    public class ChatPartisipants
    { 
        public Queue ChatPartisipantsNames { get; private set; }
        public string ChatName { get; private set; }
        public string  Message { get; set; }
        public ChatPartisipants(string chatname, List<string> username)
        {
            if (string.IsNullOrWhiteSpace(chatname))
                throw new ArgumentException("message", nameof(chatname));
            ChatPartisipantsNames = new Queue();
            ChatName = chatname;
            foreach (var item in username)
            {
                ChatPartisipantsNames.Enqueue(item);
            }
        }
        ~ChatPartisipants()
        {
            Console.WriteLine("Successfully leaved chat!");
        }
        public void SendMessage(string message,string fromuser)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("message", nameof(message));
            Message = message;
            foreach (var toUserName in ChatPartisipantsNames)
            {
                Helper.SendMessageTopic(toUserName.ToString(), fromuser, message);
            }
            Console.WriteLine("\n**Message Sent!**");
        }
        public void AddPeopleToChat(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("message", nameof(username));
            ChatPartisipantsNames.Enqueue(username);
        }

    }
}
