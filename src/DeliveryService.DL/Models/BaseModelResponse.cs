using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DeliveryService.DL.Models
{
    public class BaseModelResponse
    {
        [Required]
        public int Id { get; set; }
    }
}
