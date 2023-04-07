using Azure.Core;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Start our own EventHub Producer");
            string nameSpaceConnectionString = "Endpoint=sb://eventhubtestpersonal.servicebus.windows.net/;SharedAccessKeyName=sendandreceive;SharedAccessKey=U5un8lvQTaKo2Xd8U/4IMVAhXhINmppVv+AEhPQhIt4=;EntityPath=testeventhub";
            string eventHubName = "testeventhub";

            //await SendEnumerableEvents(nameSpaceConnectionString, eventHubName);
            await SendEnumerableEventsInBatch(nameSpaceConnectionString, eventHubName);

            Console.WriteLine("Sent out the events...");
            Console.ReadLine();
        }

        private static async Task SendEnumerableEvents(string nameSpaceConnectionString, string eventHubName)
        {
            EventHubProducerClient producer = new EventHubProducerClient(nameSpaceConnectionString, eventHubName);
            List<EventData> events = new List<EventData>();

            for (int i = 0; i < 10; i++)
            {
                events.Add(new EventData($"This is event {i}"));
            }

            await producer.SendAsync(events);

        }

        private static async Task SendEnumerableEventsInBatch(string nameSpaceConnectionString, string eventHubName)
        {
            EventHubProducerClient producer = new EventHubProducerClient(nameSpaceConnectionString, eventHubName);
            var batch = await producer.CreateBatchAsync();

            for (int i = 0; i < 10; i++)
            {
                batch.TryAdd(new EventData($"This is event in batch: event {i}"));
            }

            await producer.SendAsync(batch);
        }
    }
}
