using System.ComponentModel.DataAnnotations;

namespace DeliveryService.API.Models.Warehouses
{
    public class WarehouseRequestModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
