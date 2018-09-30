using DeliveryService.DAL.Models;
using System.Collections.Generic;

namespace DeliveryService.DL.Models
{
    public class RouteResponse
    {
        public Warehouse Origin { get; set; }
        public Warehouse Destiny { get; set; }
        public IEnumerable<Warehouse> RoutePoints { get; set; }
        public IEnumerable<SHIPS_TO> ShipDetails { get; set; }
        public int TotalCost { get; set; }
        public int TotalTime { get; set; }
        public int Hops { get; set; }
    }
}
