
namespace DeliveryService.DL.Helpers
{
    public static class RouteOptions
    {
        //sort cypher query
        public static readonly string SortByTime = "TotalTime";
        public static readonly string SortByCost = "TotalCost";
        public static readonly string SortByLength ="Hops";

        //possible routes
        public const string BestPath = "best";
        public const string QuickestPath = "quick";
        public const string SlownessPath = "slow";
        public const string CheapestPath = "cheap";
        public const string CostliesPath = "expensive";
        public const string ShortestPath = "short";
        

        public static string[] GetSortOptions()
        {
            return new string[] { SortByTime, SortByCost, SortByLength };
        }
        public static string GetValidOptions()
        {
            return string.Join(',', GetSortOptions());
        }
        public static string GetPathOptions()
        {
            return string.Join(',', BestPath,QuickestPath,SlownessPath,CheapestPath,CostliesPath,ShortestPath);
        }
    }
}
