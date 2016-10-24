using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ReadEventHub
{
    class Program
    {
        //static void Main(string[] args)
        //{
            //var eventHubClient =
            //    EventHubClient.CreateFromConnectionString(
            //        "Endpoint=sb://eventhubkkociuga.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=lnzq18SFfgQuAdKmzwjP8Eh0ig5puRmC7/C5VGeN06s=",
            //        "eventhubforemail");

            //var path = eventHubClient.Path;
            //var eventHubConsumerGroup = eventHubClient.GetConsumerGroup("Default");
            //var eventHubConsumerGroup2 = eventHubClient.GetConsumerGroup("$Default");

            //var eventHubReceiver = eventHubConsumerGroup2.CreateReceiver("1");
            //var received = eventHubReceiver.Receive();
            //var s = received.GetBodyStream();
            //var t = received.GetBytes();
            //var s1 = Encoding.UTF8.GetString(t);
        //}

        static void Main(string[] args)
        {
            string eventHubConnectionString = "Endpoint=sb://eventhubkkociuga.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=lnzq18SFfgQuAdKmzwjP8Eh0ig5puRmC7/C5VGeN06s=";
            string eventHubName = "eventhubforemail";
            string storageAccountName = "storageaccountkkociuga";
            string storageAccountKey = "hV+7dHyJtG6fYqfrFQVz2tfNczRHLxasXlUuw9+eIrPGFkpEgYr5mdZW5AUjb1dXTCGkOUbWWzw0k6YIO/Dmew==";
            string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageAccountName, storageAccountKey);

            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, eventHubName, EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);
            Console.WriteLine("Registering EventProcessor...");
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(options).Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
