//Create by Tammy Le
using System;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace VibeDrop.Model
{
    public class TodoItem
    {

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get; set;
        }

        [JsonProperty(PropertyName = "text")]
        public string Name
        {
            get; set;
        }

        [JsonProperty(PropertyName = "complete")]
        public bool Done
        {
            get; set;
        }

        [Version]
        public string Version { get; set; }
    }

}
