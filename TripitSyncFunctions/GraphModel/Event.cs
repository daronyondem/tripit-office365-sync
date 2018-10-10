using Newtonsoft.Json;
using System;

namespace TripitSyncFunctions.GraphModel
{
    public partial class Event
    {
        [JsonProperty("Id")]
        public object Id { get; set; }

        [JsonProperty("Subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        [JsonProperty("Start", NullValueHandling = NullValueHandling.Ignore)]
        public End Start { get; set; }

        [JsonProperty("End", NullValueHandling = NullValueHandling.Ignore)]
        public End End { get; set; }

        [JsonProperty("ShowAs", NullValueHandling = NullValueHandling.Ignore)]
        public string ShowAs { get; set; }

        [JsonProperty("IsAllDay", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAllDay { get; set; }

        [JsonProperty("Body", NullValueHandling = NullValueHandling.Ignore)]
        public Body Body { get; set; }

        [JsonProperty("IsReminderOn", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsReminderOn { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("ContentType", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; }

        [JsonProperty("Content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }
    }

    public partial class End
    {
        [JsonProperty("DateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DateTime { get; set; }

        [JsonProperty("TimeZone", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeZone { get; set; }
    }

    public partial class Event
    {
        public static Event FromJson(string json) => JsonConvert.DeserializeObject<Event>(json, TripitSyncFunctions.GraphModel.Converter.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this Event self) => JsonConvert.SerializeObject(self, TripitSyncFunctions.GraphModel.Converter.Settings);
    }
}