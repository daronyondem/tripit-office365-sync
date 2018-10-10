using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TripitSyncFunctions.iCalModel;
using TripitSyncFunctions.Model;
using Microsoft.Extensions.Configuration;
using System.Net;
using TripitSyncFunctions.GraphModel;
using System.Globalization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TripitSyncFunctions.TableServices;

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

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["Storage"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable tokenTable = tableClient.GetTableReference("tokenTable");
            //TODO:Iterate through tenants instead of getting the top one.
            TableQuery<TokenEntity> query = new TableQuery<TokenEntity>().Take(1);
            TableQuerySegment<TokenEntity> resultSegment = tokenTable.ExecuteQuerySegmentedAsync(query, null).Result;

            TokenEntity currentTokenEntity = resultSegment.Results.FirstOrDefault();
            var tripItUrl = currentTokenEntity.TripItUrl;
            var access_token = currentTokenEntity.AccessToken;

            var result = string.Empty;
            using (var webClient = new System.Net.WebClient())
            {
                result = webClient.DownloadString(tripItUrl);
            }

            vCalendar vcalendar = new vCalendar(result);
            List<CalendarEvent> calendarList = new List<CalendarEvent>();

            foreach (var item in vcalendar.vEvents)
            {
                if (!DateTime.TryParseExact(item.ContentLines["DTSTART"].Value, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDateTime))
                {
                    DateTime.TryParseExact(item.ContentLines["DTSTART"].Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime);
                }
                if (!DateTime.TryParseExact(item.ContentLines["DTEND"].Value, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDateTime))
                {
                    DateTime.TryParseExact(item.ContentLines["DTEND"].Value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime);
                }

                Event newCalenderEvent = new Event
                {
                    Subject = item.ContentLines["SUMMARY"].Value,
                    Location = new Location() { DisplayName = item.ContentLines["LOCATION"].Value, Address = null },
                    Start = new EventTime() { DateTime = startDateTime, TimeZone = "Etc/GMT" },
                    End = new EventTime() { DateTime = endDateTime, TimeZone = "Etc/GMT" },
                    Body = new Body() { Content = item.ContentLines["DESCRIPTION"].Value, ContentType = "Text" },
                    ShowAs = "Busy",
                    IsAllDay = false,
                    IsReminderOn = true
                };

                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    webClient.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {access_token}");
                    try
                    {
                        var response = webClient.UploadString("https://graph.microsoft.com/v1.0/me/calendar/events", newCalenderEvent.ToJson());
                    }
                    catch (WebException exception)
                    {
                        var responseStream = exception.Response?.GetResponseStream();
                        if (responseStream != null)
                        {
                            using (var reader = new System.IO.StreamReader(responseStream))
                            {
                                Error apiError = Error.FromJson(reader.ReadToEnd());
                                if(apiError.ErrorError.Message.Contains("expired"))
                                {
                                    //TODO:Refresh token
                                }
                            }
                        }
                    }

                }
            }
        }
    }
}
