using System;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace VibeDrop.Model
{
    public class TodoItem
    {

        [JsonProperty(PropertyName = "d")]
        public string Id
        {
            get; set;
        }

        [Newtonsoft.Json.JsonProperty("userId")]
        public string UserId { get; set; }
        public DateTime DateUtc { get; set; }
        public string OS { get; set; }
        public bool Complete { get; set; }
        public bool Done { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

    }
}
