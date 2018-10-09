
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TripitSyncFunctions.Model;
using TripitSyncFunctions.TableServices;

namespace TripitSyncFunctions
{
    public static class Tokenize
    {
        private const string TokenizationUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";

        [FunctionName("Tokenize")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            string code = req.Query["code"];
            var returnUrl = "http://" + req.Host + "/api/tokenize";
            var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            var clientId = config["AppClientId"];
            var clientSecret = config["AppSecret"];

            var tokenizationRequestBody = $"grant_type=authorization_code&code={code}&redirect_uri={System.Net.WebUtility.UrlEncode(returnUrl)}&client_id={clientId}&client_secret={System.Net.WebUtility.UrlEncode(clientSecret)}";

            AuthToken token = null;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string result = wc.UploadString(TokenizationUrl, tokenizationRequestBody);
                token = AuthToken.FromJson(result);
            }
            if(token!=null)
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(token.IdToken);

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["Storage"]);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable tokenTable = tableClient.GetTableReference("tokenTable");
                TokenEntity tokenEntity = new TokenEntity()
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    ADObjectId = jwtToken.Claims.FirstOrDefault(x => x.Type == "oid")?.Value,
                    ADTenantId = jwtToken.Claims.FirstOrDefault(x => x.Type == "tid")?.Value,
                    PartitionKey= jwtToken.Claims.FirstOrDefault(x => x.Type == "tid")?.Value,
                    RowKey = jwtToken.Claims.FirstOrDefault(x => x.Type == "oid")?.Value
                };
                TableOperation insertOperation = TableOperation.InsertOrMerge(tokenEntity);
                await tokenTable.ExecuteAsync(insertOperation);

                using (StreamReader sr = new StreamReader(System.IO.Path.Combine(context.FunctionDirectory, "..\\Assets\\tripit-uri-input.html")))
                {
                    string html = await sr.ReadToEndAsync().ConfigureAwait(false);
                    html = html.Replace("{tenantId}", tokenEntity.ADTenantId).Replace("{objectId}", tokenEntity.ADObjectId);
                    return new ContentResult()
                    {
                        Content = html,
                        ContentType = "text/html",
                    };
                }
            }
            else
            {
                log.LogInformation($"Auth token null received at: {DateTime.Now}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
