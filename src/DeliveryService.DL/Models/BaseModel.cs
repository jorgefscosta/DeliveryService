using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DeliveryService.DL.Models
{
    public class BaseModel
    {
        [Required]
        public int Id { get; set; }
    }
}
