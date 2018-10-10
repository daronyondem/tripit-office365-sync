using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TripitSyncFunctions.GraphModel
{
    public partial class EventResponse
    {
        [JsonProperty("@odata.context", NullValueHandling = NullValueHandling.Ignore)]
        public Uri OdataContext { get; set; }

        [JsonProperty("@odata.etag", NullValueHandling = NullValueHandling.Ignore)]
        public string OdataEtag { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("createdDateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? CreatedDateTime { get; set; }

        [JsonProperty("lastModifiedDateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? LastModifiedDateTime { get; set; }

        [JsonProperty("changeKey", NullValueHandling = NullValueHandling.Ignore)]
        public string ChangeKey { get; set; }

        [JsonProperty("categories", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Categories { get; set; }

        [JsonProperty("originalStartTimeZone", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalStartTimeZone { get; set; }

        [JsonProperty("originalEndTimeZone", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalEndTimeZone { get; set; }

        [JsonProperty("iCalUId", NullValueHandling = NullValueHandling.Ignore)]
        public string ICalUId { get; set; }

        [JsonProperty("reminderMinutesBeforeStart", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReminderMinutesBeforeStart { get; set; }

        [JsonProperty("isReminderOn", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsReminderOn { get; set; }

        [JsonProperty("hasAttachments", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasAttachments { get; set; }

        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        [JsonProperty("bodyPreview", NullValueHandling = NullValueHandling.Ignore)]
        public string BodyPreview { get; set; }

        [JsonProperty("importance", NullValueHandling = NullValueHandling.Ignore)]
        public string Importance { get; set; }

        [JsonProperty("sensitivity", NullValueHandling = NullValueHandling.Ignore)]
        public string Sensitivity { get; set; }

        [JsonProperty("isAllDay", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAllDay { get; set; }

        [JsonProperty("isCancelled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCancelled { get; set; }

        [JsonProperty("isOrganizer", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsOrganizer { get; set; }

        [JsonProperty("responseRequested", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ResponseRequested { get; set; }

        [JsonProperty("seriesMasterId")]
        public object SeriesMasterId { get; set; }

        [JsonProperty("showAs", NullValueHandling = NullValueHandling.Ignore)]
        public string ShowAs { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("webLink", NullValueHandling = NullValueHandling.Ignore)]
        public Uri WebLink { get; set; }

        [JsonProperty("onlineMeetingUrl")]
        public object OnlineMeetingUrl { get; set; }

        [JsonProperty("recurrence")]
        public object Recurrence { get; set; }

        [JsonProperty("responseStatus", NullValueHandling = NullValueHandling.Ignore)]
        public ResponseStatus ResponseStatus { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public ResponseBody Body { get; set; }

        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public EventTimeResponse Start { get; set; }

        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public EventTimeResponse End { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public LocationResponse Location { get; set; }

        [JsonProperty("locations", NullValueHandling = NullValueHandling.Ignore)]
        public List<LocationResponse> Locations { get; set; }

        [JsonProperty("attendees", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Attendees { get; set; }

        [JsonProperty("organizer", NullValueHandling = NullValueHandling.Ignore)]
        public Organizer Organizer { get; set; }
    }

    public partial class ResponseBody
    {
        [JsonProperty("contentType", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }
    }

    public partial class EventTimeResponse
    {
        [JsonProperty("dateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DateTime { get; set; }

        [JsonProperty("timeZone", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeZone { get; set; }
    }

    public partial class LocationResponse
    {
        [JsonProperty("displayName", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("locationType", NullValueHandling = NullValueHandling.Ignore)]
        public string LocationType { get; set; }

        [JsonProperty("uniqueId", NullValueHandling = NullValueHandling.Ignore)]
        public string UniqueId { get; set; }

        [JsonProperty("uniqueIdType", NullValueHandling = NullValueHandling.Ignore)]
        public string UniqueIdType { get; set; }
    }

    public partial class Organizer
    {
        [JsonProperty("emailAddress", NullValueHandling = NullValueHandling.Ignore)]
        public EmailAddress EmailAddress { get; set; }
    }

    public partial class EmailAddress
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    public partial class ResponseStatus
    {
        [JsonProperty("response", NullValueHandling = NullValueHandling.Ignore)]
        public string Response { get; set; }

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Time { get; set; }
    }

    public partial class EventResponse
    {
        public static EventResponse FromJson(string json) => JsonConvert.DeserializeObject<EventResponse>(json, TripitSyncFunctions.GraphModel.Converter.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this EventResponse self) => JsonConvert.SerializeObject(self, TripitSyncFunctions.GraphModel.Converter.Settings);
    }
}