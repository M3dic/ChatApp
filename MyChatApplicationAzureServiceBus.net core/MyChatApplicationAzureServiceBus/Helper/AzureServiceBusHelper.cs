using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServiceBus
{
    /// <summary>
    /// Method that provide useful methods for the Chat Service Bus software
    /// </summary>
    public class AzureServiceBusHelper
    {
        //Retrieve the connection string
        private static readonly string ConnectionString = "Endpoint=sb://chatserviceapp.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=++EgzIb8X+oMd9w+vLluwTLUyPHKT8y2VP1mo4EcJM0=";
        //Retrieve the topic's name
        private static readonly string TopicName = "1chatperform";

        public static SubscriptionClient Client { get; private set; }

        public static List<string> TakeAllTopicsForUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));
            List<string> topiclist = new List<string>();
            if (ConnectionString != null)
            {
                var namespaceManager = new
                  ManagementClient(ConnectionString);
                List<TopicDescription> topics = namespaceManager.GetTopicsAsync().Result.ToList();
                foreach (var item in topics)
                    if (item.Path.ToString().Split('1').ToList().Contains(username))
                        topiclist.Add(item.Path.ToString());
            }
            return topiclist;
        }

        /// <summary>
        /// Method to create a topic for the first configuration
        /// </summary>
        public static void CreateTopic(string TopicName)
        {
            if (string.IsNullOrWhiteSpace(TopicName))
                throw new ArgumentNullException(nameof(TopicName));

            //Retrieve the connection string
            if (ConnectionString != null)
            {
                //connection string not null, get the namespace manager
                var namespaceManager = new
                    ManagementClient(ConnectionString);
                // Create the topic if it does not exist already.
                try
                {
                    if (namespaceManager.GetTopicAsync(TopicName).Result.Status == EntityStatus.Unknown ||
                   namespaceManager.GetTopicAsync(TopicName).Result.Status == EntityStatus.Disabled)
                    {
                        TopicDescription td = new TopicDescription(TopicName);
                        namespaceManager.CreateTopicAsync(td);
                    }
                }
                catch (Exception)
                {
                    TopicDescription td = new TopicDescription(TopicName);
                    namespaceManager.CreateTopicAsync(td);
                }

            }
        }


        /// <summary>
        /// Get all the subscription of the chat topic (it will be the user names)
        /// </summary>
        /// <returns>the list of subscription</returns>
        public List<SubscriptionDescription> GetSubscriptionsNames(string TopicName)
        {

            if (ConnectionString != null)
            {
                //connection string not null, get the namespace manager
                var namespaceManager = new
                    ManagementClient(ConnectionString);

                //validate topic exist
                if (namespaceManager.GetTopicAsync(TopicName).Result.Status != EntityStatus.Unknown ||
                    namespaceManager.GetTopicAsync(TopicName).Result.Status != EntityStatus.Disabled)
                {
                    //return the list of all subscriptions of this topic
                    return namespaceManager.GetSubscriptionsAsync(TopicName).Result.ToList();
                }
            }
            //we can't retrieve it, connection string is missing
            return null;
        }

        /// <summary>
        /// Method to create a subscription when a new user is coming
        /// </summary>
        /// <param name="UserNames">the name of the suscription ,which will be the user name</param>
        public static void CreateSubscription(string TopicName, List<string> UserNames)
        {
            if (UserNames == null) throw new ArgumentNullException(nameof(UserNames));

            if (ConnectionString != null)
            {
                //connection string not null, get the namespace manager
                var namespaceManager = new
                ManagementClient(ConnectionString);

                //Create new subscription
                foreach (var username in UserNames)
                {
                    namespaceManager.CreateSubscriptionAsync(TopicName, username.ToLowerInvariant());
                }
            }
        }

        /// <summary>
        /// Method to send a message to the topic
        /// </summary>
        /// <param name="toUserName">the username to who send a message</param>
        /// <param name="messageContent">the content of the message</param>
        public static void SendMessageTopic(string TopicName, string toUserName, string fromUserName, string messageContent)
        {
            if (string.IsNullOrWhiteSpace(toUserName))
                throw new ArgumentNullException(nameof(toUserName));

            if (string.IsNullOrWhiteSpace(fromUserName))
                throw new ArgumentNullException(nameof(fromUserName));

            if (string.IsNullOrWhiteSpace(messageContent))
                throw new ArgumentNullException(nameof(messageContent));

            //to avoid any typo, lower the string
            string toUserNameLow = toUserName.ToLowerInvariant();

            if (ConnectionString != null)
            {
                //create the TopicClient object
                var Client = new
                    TopicClient(ConnectionString, TopicName);

                Message message = new Message(Encoding.ASCII.GetBytes(fromUserName + ": " + messageContent));
                message.UserProperties["UserName"] = toUserNameLow;
                Client.SendAsync(message);

            }
        }


        /// <summary>
        /// Method to Receive a message from a subscription
        /// </summary>
        /// <param name="userName">the username of the user to get messages</param>
        public static void ReceiveMessageSubscription(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            //to avoid any typo, lower the string
            string userNameLow = userName.ToLowerInvariant();

            if (ConnectionString != null)
            {

                Task.Run(() =>
               {
                   try
                   {
                       Client = new SubscriptionClient(ConnectionString, TopicName, userName);

                       // Configure message handler
                       // Values are default but set for illustration
                       var messageHandlerOptions =
                          new MessageHandlerOptions(ExceptionReceivedHandler)
                          {
                              AutoComplete = false,
                              // ExceptionReceivedHandler = set in constructor
                              MaxAutoRenewDuration = new TimeSpan(0, 5, 0), // 5 minutes
                              MaxConcurrentCalls = 1,
                          };

                       // Register the function that processes messages.
                       // Once set new message will be received
                       Client.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine("Error: " + ex.Message + ", " + ex.StackTrace);
                   }

                   Console.ReadKey();

                   Client.CloseAsync();
               });
            }
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            Console.WriteLine("Received message: \n" + Encoding.UTF8.GetString(message.Body));

            // This will complete the message, other options are availalbe
            await Client.CompleteAsync(message.SystemProperties.LockToken);

        }

        // Use handler to examine the exceptions received on the message pump.
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exReceivedEventArgs)
        {
            // The exception context reveals what happened!
            var exContext = exReceivedEventArgs.ExceptionReceivedContext;
            //var msg = "Exception Endpoint: " + exContext.Endpoint + ", Action: " + exContext.Action;
            //Console.WriteLine(msg);

            return Task.CompletedTask;
        }
    }
}
