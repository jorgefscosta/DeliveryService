using Newtonsoft.Json;

namespace DeliveryService.DAL.Models
{
    public class Warehouse : NodeEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
