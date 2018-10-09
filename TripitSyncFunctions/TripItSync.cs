using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TripitSyncFunctions.iCalModel;
using TripitSyncFunctions.Model;
using Microsoft.Extensions.Configuration;

namespace TripitSyncFunctions
{
    public static class TripItSync
    {
        [FunctionName("TripItSync")]
        public static void Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                        .SetBasePath(context.FunctionAppDirectory)
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();
            var tripItUrl = config["TripItiCalUrl"];

            var result = string.Empty;
            using (var webClient = new System.Net.WebClient())
            {
                result = webClient.DownloadString(tripItUrl);
            }

            vCalendar vcalendar = new vCalendar(result);
            List<CalendarEvent> calendarList = new List<CalendarEvent>();

            foreach (var item in vcalendar.vEvents)
            {
                var newEvent = new CalendarEvent
                {
                    Title = item.ContentLines["SUMMARY"].Value,
                    Location = item.ContentLines["LOCATION"].Value,
                    Start = item.ContentLines["DTSTART"].Value,
                    End = item.ContentLines["DTEND"].Value,
                    Body = item.ContentLines["DESCRIPTION"].Value
                };
                calendarList.Add(newEvent);
            }

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
