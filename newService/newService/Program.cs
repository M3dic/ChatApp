using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace SBQueueReader
{
    class Program
    {
        static IQueueClient queueClient;

        static async Task Main(string[] args)
        {
            var sbconnstr = "Use primary connection from your sb namespace";
            var qname = "sbqueue";

            await Task.Run(() =>
            {
                try
                {
                    queueClient = new QueueClient(sbconnstr, qname);

                    // Configure message handler, set the exception handler
                    // Default values are shown for illustration
                    var messageHandlerOptions =
new MessageHandlerOptions(ExceptionHandler)
{
    AutoComplete = false,
    MaxAutoRenewDuration = new TimeSpan(0, 5, 0), // 5 minutes
    MaxConcurrentCalls = 1,
};

                    // Register the function that will process messages
                    queueClient.RegisterMessageHandler(MessageProcessor,
messageHandlerOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + ", "
+ ex.StackTrace);
                }

                Console.ReadKey();

                queueClient.CloseAsync();
            });
        }

        static async Task MessageProcessor(Message message,
CancellationToken token)
        {
            Console.WriteLine("Received message: "
+ Encoding.UTF8.GetString(message.Body));
            // complete the message
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
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