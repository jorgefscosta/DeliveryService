using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DL.Models
{
    public class ShipsToRequest
    {
        public int OriginId { get; set; }
        public int DestinyId { get; set; }
        public int Time { get; set; }
        public int Cost { get; set; }
    }
}
