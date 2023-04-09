using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHubTestReceiveEvent
{
    public class Program
    {
        private readonly static string nameSpaceConnectionString = "Endpoint=sb://eventhubtestpersonal.servicebus.windows.net/;SharedAccessKeyName=sendandreceive;SharedAccessKey=U5un8lvQTaKo2Xd8U/4IMVAhXhINmppVv+AEhPQhIt4=;EntityPath=testeventhub";
        private readonly static string eventHubName = "testeventhub";

        private readonly static string blobConnectionString = "DefaultEndpointsProtocol=https;AccountName=eventhubteststorage2;AccountKey=cacwGETegYX6t/I73k84o381ABPsqSGBYogHbsPyA9iAOxJomm8HckY9ysLOIR3WvdfXUrsWAL3++AStMPc7Ng==;EndpointSuffix=core.windows.net";
        private readonly static string containerName = "offsetcontainer";
        private readonly static string consumerGroup = "$Default";

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Start Event Hub Receiver...");
            /** 
             * the blob container client is used to store the check point. (check point can be used to record where we consumed the event last time and 
             * use the UpdateCheckPointAsync method to avoid repeated consumption.
             * */
            BlobContainerClient blobContainerClient = new BlobContainerClient(blobConnectionString, containerName);
            EventProcessorClient processor = new EventProcessorClient(blobContainerClient, consumerGroup, nameSpaceConnectionString, eventHubName);

            processor.ProcessEventAsync += Processor_ProcessEventAsync;
            processor.ProcessErrorAsync += Processor_ProcessErrorAsync;

            await processor.StartProcessingAsync();
            Console.WriteLine("Start the processor");

            Console.ReadLine();
            await processor.StopProcessingAsync();
            Console.WriteLine("Stop the processor");
            
        }

        private static Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine("Error Received: " + arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private static async Task Processor_ProcessEventAsync(ProcessEventArgs arg)
        {
            Console.WriteLine($"Event Received from Partition {arg.Partition.PartitionId}: {arg.Data.EventBody.ToString()}");

            // this is to tell EvenHub to get the offset from blob container and read through eventhub where we consume last time
            // and make sure there won't be the repeated consumption.
            await arg.UpdateCheckpointAsync();
        }
    }
}
