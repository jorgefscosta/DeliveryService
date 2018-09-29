using DeliveryService.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DL.Models
{
    public class Route
    {
        public Warehouse StartPoint { get; set; }
        public Warehouse EndPoint { get; set; }
        public IEnumerable<Warehouse> RoutePoints { get; set; }
        public IEnumerable<ShipsTo> ShipDetails { get; set; }
        public int Hops { get; set; }
        public int TotalCost { get; set; }
        public int TotalTime { get; set; }
    }
}
