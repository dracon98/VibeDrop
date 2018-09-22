using System;
namespace VibeDrop.Model
{
    //Created by Tammy Le, 23/9/2018

    public class Note
    {
        //declare the ID
        [Newtonsoft.Json.JsonProperty("ID")]
        public string ID { get; set; }

        //declare the UserID property
        [Newtonsoft.Json.JsonProperty("UserID")]
        public string UserId { get; set; }


        //declare the Date UTC
        public DateTime DateUTC { get; set; }

        //declare property location - MadeAtHome
        public bool MadeAtHome { get; set; }

        //declare property location - other locations
        public string Location { get; set; }

        //declare the Operating System 
        public string OS { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string DateDisplay { get { return DateUTC.ToLocalTime().ToString("date"); } }

        [Newtonsoft.Json.JsonIgnore]
        public string TimeDisplay { get { return DateUTC.ToLocalTime().ToString("time") + " " + OS.ToString(); } }

        [Newtonsoft.Json.JsonIgnore]
        public string AtHomeDisplay { get { return MadeAtHome ? "Create At Home" : Location; } }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        public Note()
        {
            
        }
    }
}
