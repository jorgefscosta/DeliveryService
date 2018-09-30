using DeliveryService.DAL.Models.Relationships;
using Newtonsoft.Json;

namespace DeliveryService.DAL.Models
{
    public class SHIPS_TO : RelationshipEntity
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }
    }
}
