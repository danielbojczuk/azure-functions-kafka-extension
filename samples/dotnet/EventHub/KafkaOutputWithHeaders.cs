using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;

namespace Eventhub
{
    public class KafkaOutputWithHeaders
    {
        [FunctionName("KafkaOutputWithHeaders")]
        public static IActionResult Output(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Kafka("BrokerList",
                    "topic",
                    Username = "$ConnectionString",
                    Password = "%EventHubConnectionString%",
                    Protocol = BrokerProtocol.SaslSsl,
                   AuthenticationMode = BrokerAuthenticationMode.Plain
            )] out KafkaEventData<string> eventData,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string message = req.Query["message"];        
            eventData = new KafkaEventData<string>(message);
            eventData.Headers.Add("test", System.Text.Encoding.UTF8.GetBytes("dotnet")); 

            return new OkObjectResult("Ok");
        }
    }
}