﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TripitSyncFunctions.Model
{
    public partial class AuthToken
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("ext_expires_in")]
        public long ExtExpiresIn { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }

    public partial class AuthToken
    {
        public static AuthToken FromJson(string json) => JsonConvert.DeserializeObject<AuthToken>(json, Model.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this AuthToken self) => JsonConvert.SerializeObject(self, Model.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
