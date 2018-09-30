namespace DeliveryService.DL.Models
{
    public class ShipsToRequest
    {
        public WarehouseResponse Origin { get; set; }
        public WarehouseResponse Destiny { get; set; }
        public int Time { get; set; }
        public int Cost { get; set; }
    }
}
