
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

namespace TripitSyncFunctions
{
    //Register your app at https://apps.dev.microsoft.com
    public static class Authorize
    {
        [FunctionName("Authorize")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                       .SetBasePath(context.FunctionAppDirectory)
                       .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                       .AddEnvironmentVariables()
                       .Build();
            var clientId = config["AppClientId"];
            var returnUrl = "http://" + req.Host + "/api/tokenize";

            var authorizationUrl = $"https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id={clientId}&redirect_uri={System.Net.WebUtility.UrlEncode(returnUrl)}&response_type=code&scope=openid%20profile%20offline_access%20user.read%20Calendars.ReadWrite&nonce={Guid.NewGuid().ToString("N")}";

            return new RedirectResult(authorizationUrl);
        }
    }
}
