using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;

namespace My.Function
{
    public class ViewCount : TableEntity
    {
        public ViewCount(string URL)
        {
            this.PartitionKey = URL; //the partition key for load-balancing and identification
            this.RowKey = "visits"; // hardcoded for identification
            Count = 0; // the value we want to store
        }

        public int Count { get; set; }

        public ViewCount()
        {
            Count = 0;
        }
    }

    public static class ConsCount02
    {
        const string tableName = "viewcountertable";

        [FunctionName("ConsCount02")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var storageAccountConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
            var storageAccount = CloudStorageAccount.Parse($"{storageAccountConnectionString}");
            var tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            await table.CreateIfNotExistsAsync(); // we can let our code create the table if needed

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string pageViewURL = data?.URL;

            if (pageViewURL == null)
            {
                return (ActionResult)new StatusCodeResult(503);
            }

            var retrievedResult = table.Execute(TableOperation.Retrieve<ViewCount>(pageViewURL, "visits"));
            var pageView = (ViewCount)retrievedResult.Result;

            pageView = pageView ?? new ViewCount(pageViewURL);

            pageView.Count++;

            table.Execute(TableOperation.InsertOrReplace(pageView));
            return (ActionResult)new OkObjectResult(pageView.Count.ToString());
        }
    }
}

// dotnet add package Microsoft.Azure.Cosmos.Table
// DefaultEndpointsProtocol=[http|https];AccountName=myAccountName;AccountKey=myAccountKey
//Allow static site on CORS - function app (https://pwd9000staticsite.z33.web.core.windows.net)
