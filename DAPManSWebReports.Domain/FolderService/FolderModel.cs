﻿using System.Text.Json.Serialization;

namespace DAPManSWebReports.Domain.FolderService
{
    public class FolderModel
    {
        public int id { get; set; }

        [JsonPropertyName("parentid")]
        public int Parentid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("system")]
        public bool System { get; set; }

        [JsonPropertyName("remoteUser")]
        public string RemoteUser { get; set; }

        [JsonPropertyName("remotePassword")]
        public string RemotePassword { get; set; }

        [JsonPropertyName("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonPropertyName("lastUser")]
        public string LastUser { get; set; }

    }
}
