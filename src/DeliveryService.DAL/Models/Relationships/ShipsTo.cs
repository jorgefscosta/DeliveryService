using DeliveryService.DAL.Models.Relationships;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DAL.Models
{
    public class ShipsTo
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }
    }
}
