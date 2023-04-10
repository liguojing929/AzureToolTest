

namespace EventHubTestSendEvent
{
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.KeyVault;

    public class Program
    {
        //private readonly static string nameSpaceConnectionString = "Endpoint=sb://eventhubtestpersonal.servicebus.windows.net/;SharedAccessKeyName=sendandreceive;SharedAccessKey=U5un8lvQTaKo2Xd8U/4IMVAhXhINmppVv+AEhPQhIt4=;EntityPath=testeventhub";
        private static string nameSpaceConnectionString;
        private readonly static string eventHubName = "testeventhub";
        private static KeyVaultCommon keyVaultTool;

        public static async Task Main(string[] args)
        {
            keyVaultTool = new KeyVaultCommon();
            Console.WriteLine("Start the EventHub Producer.");
            nameSpaceConnectionString = await keyVaultTool.GetSecreteValue("EventHubNameSpaceConnectionString-Personal");
            Console.WriteLine($"nameSpaceConnectionString is: {nameSpaceConnectionString}");
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
                batch.TryAdd(new EventData($"This is event in batch: {i}"));
            }

            await producer.SendAsync(batch);
        }
    }
}
