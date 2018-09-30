
namespace DeliveryService.DL.Helpers
{
    public class RouteSetup
    {
        public string MinHops { get; }
        public string MaxHops { get; }
        public RouteSetup(string min, string max)
        {
            MinHops = min;
            MaxHops = max;
        }
    }

}
