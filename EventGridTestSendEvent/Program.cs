using Azure;
using Azure.Messaging.EventGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EventGridTestSendEvent
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Start to send event to Event Grid...");
            await SendEventToEventGrid();
            Console.WriteLine("Finish Sending event to Event Grid...");
            Console.ReadLine();
        }

        private static async Task SendEventToEventGrid()
        {
            // This is the Event Grid Topic Endpoint
            Uri endPoint = new Uri("https://test-topic-1.eastasia-1.eventgrid.azure.net/api/events");

            // This is the Event Grid Topic Access Key
            string accessKey = "CmyLM6cymTPSO8ch5dxXQfmwMFmfp7fYzQQIHCLEXcc=";

            // Create an instance of EventGridPublisherClient
            EventGridPublisherClient client = new EventGridPublisherClient(endPoint, new AzureKeyCredential(accessKey));

            // Create Event Grid Event
            EventGridEvent eventGridEvent = new EventGridEvent(
                subject: "test-subject",
                eventType: "test-type",
                dataVersion: "1.0",
                data: new
                {
                    Message = "Hello, This is a test EventGridEvent."
                });

            // Send out the event to Event Grid
            await client.SendEventAsync(eventGridEvent);
        }
    }
}
