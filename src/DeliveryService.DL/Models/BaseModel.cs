using System.ComponentModel.DataAnnotations;

namespace DeliveryService.DL.Models
{
    public class BaseModel
    {
        [Required]
        public int Id { get; set; }
    }
}
