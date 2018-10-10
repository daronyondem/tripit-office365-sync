namespace TripitSyncFunctions.GraphModel
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Error
    {
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public ErrorClass ErrorError { get; set; }
    }

    public partial class ErrorClass
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("innerError", NullValueHandling = NullValueHandling.Ignore)]
        public InnerError InnerError { get; set; }
    }

    public partial class InnerError
    {
        [JsonProperty("request-id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? RequestId { get; set; }

        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Date { get; set; }
    }

    public partial class Error
    {
        public static Error FromJson(string json) => JsonConvert.DeserializeObject<Error>(json, TripitSyncFunctions.GraphModel.Converter.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this Error self) => JsonConvert.SerializeObject(self, TripitSyncFunctions.GraphModel.Converter.Settings);
    }
}