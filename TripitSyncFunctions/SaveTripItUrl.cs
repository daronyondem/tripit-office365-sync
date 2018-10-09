
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Configuration;
using TripitSyncFunctions.TableServices;

namespace TripitSyncFunctions
{
    public static class SaveTripItUrl
    {
        [FunctionName("SaveTripItUrl")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(context.FunctionAppDirectory)
                   .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables()
                   .Build();
        
            var tenantId = req.Form["tenantId"];
            var objectId = req.Form["objectId"];
            var tripItUrl = req.Form["url"];
            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["Storage"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable tokenTable = tableClient.GetTableReference("tokenTable");
            TokenEntity tokenEntity = new TokenEntity()
            {
                PartitionKey = tenantId,
                RowKey = objectId,
                TripItUrl = tripItUrl
            };
            TableOperation updateOperation = TableOperation.InsertOrMerge(tokenEntity);
            await tokenTable.ExecuteAsync(updateOperation);

            return new ContentResult()
            {
                Content = "Done",
                ContentType = "text/html",
            };
        }
    }
}
