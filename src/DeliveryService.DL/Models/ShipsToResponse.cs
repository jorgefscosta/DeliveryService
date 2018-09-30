using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DL.Models
{
    public class ShipsToResponse
    {
        public string OriginName { get; set; }
        public string DestinyName { get; set; }
        public int Time { get; set; }
        public int Cost { get; set; }
    }
}
