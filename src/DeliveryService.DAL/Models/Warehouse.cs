using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DAL.Models
{
    public class Warehouse : BaseEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
