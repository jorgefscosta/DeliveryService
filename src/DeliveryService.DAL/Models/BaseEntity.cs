using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.DAL.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
    }
}
