
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
using Microsoft.Extensions.Configuration;
using System.Net;

namespace TripitSyncFunctions
{
    public static class Tokenize
    {
        private const string TokenizationUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        private const string TokenizationRequestBody = "grant_type=authorization_code&code={0}&redirect_uri={1}&client_id={2}&client_secret={3}";

        [FunctionName("Tokenize")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
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

            var tokenizationRequestBody = string.Format(TokenizationRequestBody, code, System.Net.WebUtility.UrlEncode(returnUrl), clientId, System.Net.WebUtility.UrlEncode(clientSecret));

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string result = wc.UploadString(TokenizationUrl, tokenizationRequestBody);
                dynamic data = JsonConvert.DeserializeObject(result);
                return (ActionResult)new OkObjectResult(data);
            }
        }
    }
}
