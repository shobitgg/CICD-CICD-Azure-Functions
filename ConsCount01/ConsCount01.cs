using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace My.Function
{
    public static class ConsCount01
    {
        [FunctionName("ConsCount01")]
       // [CosmosDBOutput("%DatabaseName%", "%CollectionName%", ConnectionStringSetting = "CosmosConnection")]
        public static object Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "%DatabaseName%",
                collectionName: "%CollectionName%",
                ConnectionStringSetting = "CosmosConnection",
                Id = "1",
                PartitionKey = "1")] CounterJson counter01,
            ILogger log)
        {
      //      counter01.Count++;

            return counter01;
        }
    }

    public class CounterJson
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public string Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
// dotnet add package Microsoft.Azure.WebJobs.Extensions.CosmosDB
// dotnet add package Microsoft.Azure.WebJobs.Extensions
// dotnet add package Microsoft.Azure.Functions.Worker.Extensions.CosmosDB
// dotnet add package Microsoft.Azure.Functions.Worker.Core
// dotnet add package Microsoft.Azure.Functions.Worker
