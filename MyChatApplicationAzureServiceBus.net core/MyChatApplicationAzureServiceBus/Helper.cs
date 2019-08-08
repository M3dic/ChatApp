using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServiceBus
{
    /// <summary>
    /// Method that provide useful methods for the Chat Service Bus software
    /// </summary>
    public class Helper
    {
        public static IQueueClient Client { get; private set; }

        /// <summary>
        /// Method to create a topic for the first configuration
        /// </summary>
        public static void CreateTopic()
        {
            //Retrieve the connection string
            string connectionString =
                ConfigurationManager.AppSettings["Service.Bus.ConnectionString"] ?? null;
            if (connectionString != null)
            {
                //connection string not null, get the namespace manager
                var namespaceManager =
                    NamespaceManager.CreateFromConnectionString(connectionString);

                //Retrieve the topic's name
                string topicName = ConfigurationManager.AppSettings["Service.Bus.Topic"];

                // Create the topic if it does not exist already.
                if (!namespaceManager.TopicExists(topicName))
                {
                    TopicDescription td = new TopicDescription(topicName);
                    namespaceManager.CreateTopic(td);
                }
            }
        }


        /// <summary>
        /// Get all the subscription of the chat topic (it will be the user names)
        /// </summary>
        /// <returns>the list of subscription</returns>
        public static List<SubscriptionDescription> GetSubscriptionsNames()
        {
            //Retrieve the connection string
            string connectionString =
               ConfigurationManager.AppSettings["Service.Bus.ConnectionString"] ?? null;

            if (connectionString != null)
            {
                //connection string not null, get the namespace manager
                var namespaceManager =
                    NamespaceManager.CreateFromConnectionString(connectionString);

                //Retrieve the topic's name
                string topicName = ConfigurationManager.AppSettings["Service.Bus.Topic"];

                //validate topic exist
                if (namespaceManager.TopicExists(topicName))
                {
                    //return the list of all subscriptions of this topic
                    return namespaceManager.GetSubscriptions(topicName).ToList();
                }
            }
            //we can't retrieve it, connection string is missing
            return null;
        }

        /// <summary>
        /// Method to create a subscription when a new user is coming
        /// </summary>
        /// <param name="userName">the name of the suscription ,which will be the user name</param>
        public static void CreateSubscription(string userName)
        {
            //to avoid any typo, lower the string
            string userNameLow = userName.ToLowerInvariant();

            //Retrieve the connection string
            var connectionString =
                  ConfigurationManager.AppSettings["Service.Bus.ConnectionString"] ?? null;

            if (connectionString != null)
            {
                //Retrieve the topic's name
                var topicName = ConfigurationManager.AppSettings["Service.Bus.Topic"];

                //connection string not null, get the namespace manager
                SubscriptionClient namespaceManager = new
                SubscriptionClient(topicName,userName);


                //check if the subscription doesn't exist already
                if (!namespaceManager.RequiresSession)
                {
                    //set the filter for this subscription
                    SqlFilter userNameFilter =
                            new SqlFilter("UserName = '" + userNameLow + "'");
                    //create the subscription
                    namespaceManager.CreateSubscription(topicName, userNameLow, userNameFilter);
                }
            }
        }

        /// <summary>
        /// Method to check if a subscription exist before sending the message
        /// </summary>
        /// <param name="toUserName">the user name of the user which we want to send a message</param>
        /// <returns>true if user is known</returns>
        public static bool IsSubscriptionExist(string toUserName)
        {
            //to avoid any typo, lower the string
            string toUserNameLow = toUserName.ToLowerInvariant();

            //Retrieve the connection string
            string connectionString =
                  ConfigurationManager.AppSettings["Service.Bus.ConnectionString"] ?? null;

            if (connectionString != null)
            {
                //connection string not null, get the namespace manager
                var namespaceManager =
                NamespaceManager.CreateFromConnectionString(connectionString);

                //Retrieve the topic's name
                string topicName = ConfigurationManager.AppSettings["Service.Bus.Topic"];

                //check if subscription exists
                return namespaceManager.SubscriptionExists(topicName, toUserNameLow);
            }
            //connection string null so return false, we can't get the needed info
            return false;
        }

        /// <summary>
        /// Method to send a message to the topic
        /// </summary>
        /// <param name="toUserName">the username to who send a message</param>
        /// <param name="messageContent">the content of the message</param>
        public static void SendMessageTopic(string toUserName, string fromUserName, string messageContent)
        {
            //to avoid any typo, lower the string
            string toUserNameLow = toUserName.ToLowerInvariant();

            //Retrieve the connection string
            string connectionString =
                    ConfigurationManager.AppSettings["Service.Bus.ConnectionString"] ?? null;

            if (connectionString != null)
            {
                //Retrieve the topic's name
                string topicName = ConfigurationManager.AppSettings["Service.Bus.Topic"];

                //create the TopicClient object
                IQueueClient Client = new
                    QueueClient(connectionString, topicName);

                //check if we have to broadcast the message or not
                if (toUserNameLow != "all")
                {
                    //no broadcast needed
                    Message message = new Message(Encoding.ASCII.GetBytes(fromUserName + ": " + messageContent));
                    message.UserProperties["UserName"] = toUserNameLow;
                    Client.SendAsync(message);
                }
                else
                {
                    //broadcasting the message
                    string fromUserNameLow = fromUserName.ToLowerInvariant();

                    //retrieving all the available subscription
                    foreach (SubscriptionDescription subDescription in GetSubscriptionsNames())
                    {
                        //check if we're not sending the message to ourself
                        if (subDescription.SubscriptionName != fromUserNameLow)
                        {
                            Message message = new Message(Encoding.ASCII.GetBytes(fromUserName + ": " + messageContent));
                            message.UserProperties["UserName"] = subDescription.SubscriptionName;
                            Client.SendAsync(message);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Method to Receive a message from a subscription
        /// </summary>
        /// <param name="userName">the username of the user to get messages</param>
        public static void ReceiveMessageSubscription(string userName)
        {
            //to avoid any typo, lower the string
            string userNameLow = userName.ToLowerInvariant();

            //Retrieve the connection string
            string connectionString =
                    ConfigurationManager.AppSettings["Service.Bus.ConnectionString"] ?? null;

            if (connectionString != null)
            {
                //Retrieve the topic's name
                string topicName = ConfigurationManager.AppSettings["Service.Bus.Topic"];

                //Create the subscription client object
                Client = new QueueClient(connectionString, topicName);

                // Configure the callback options                
                MessageHandlerOptions options = new MessageHandlerOptions(ExceptionHandler);
                //enable manual control of the method complete
                options.AutoComplete = false;
                //Every 5 seconds we check for new messages
                options.MaxAutoRenewDuration = TimeSpan.FromSeconds(5);

                //Client.SendAsync((message) =>
                //{
                //    try
                //    {
                //        // Process message from subscription
                //        Console.WriteLine("\n**Message Received!**");
                //        Console.WriteLine("\t" + message.GetBody<string>());
                //        Console.WriteLine("\n");

                //        // Remove message from subscription.
                //        message.Complete();
                //    }
                //    catch (Exception)
                //    {
                //        // Indicates a problem, unlock message in subscription
                //        message.Abandon();
                //    }
                //}, options);
                Client.RegisterMessageHandler(MessageProcessor,
options);

                Client.CloseAsync();
            }

        }
        static async Task MessageProcessor(Message message,
CancellationToken token)
        {
            Console.WriteLine("Received message: "
+ Encoding.UTF8.GetString(message.Body));
            // complete the message
            await Client.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionHandler(
ExceptionReceivedEventArgs exReceivedEventArgs)
        {
            // The exception context reveals what happened!
            var exContext = exReceivedEventArgs.ExceptionReceivedContext;

            var msg = "Exception Endpoint: " + exContext.Endpoint
+ ", Action: " + exContext.Action;
            Console.WriteLine(msg);

            return Task.CompletedTask;
        }
    }
}
