using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Embeddings;

namespace Company.Function
{
    public class KafkaTrigger1
    {
        // KafkaTrigger sample 
        // Consume the message from "topic" on the LocalBroker.
        // Add `example_APPSETTING` and `KafkaPassword` to the local.settings.json
        // For EventHubs
        // "example_APPSETTING": "{EVENT_HUBS_NAMESPACE}.servicebus.windows.net:9093"
        // "KafkaPassword":"{EVENT_HUBS_CONNECTION_STRING}
        [FunctionName("KafkaTrigger1")]
        public void Run(
            [KafkaTrigger("172.210.56.1",
                          "TutorialTopic",
                          AuthenticationMode = BrokerAuthenticationMode.Plain,
                          ConsumerGroup = "$Default")] KafkaEventData<string>[] events,
            ILogger log)
        {
            foreach (KafkaEventData<string> eventData in events)
            {
                log.LogInformation($"C# Kafka trigger function processed a message: {eventData.Value}");
                Generateembedding(eventData.Value);

            }
        }

        public   void Generateembedding(string text){
            // const  string apiendpoint = "https://api.openai.com/v1/embeddings";
            var  apikey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            const string model = "text-embedding-3-large";
            var client = new OpenAIClient(apiKey:apikey);
            var res = client.GetEmbeddingClient(model:model);
            EmbeddingGenerationOptions options = new() { Dimensions = 1536};
            var response = res.GenerateEmbedding(text,options);
           Console.WriteLine($"Embedding generated: {response}");




        }
    }
}
