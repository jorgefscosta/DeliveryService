using DeliveryService.DAL.Models;
using DeliveryService.DL.Models;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DL.Services
{
    public interface IRouteService
    {
        IEnumerable<Route> GetRoutes(Warehouse startPoint, Warehouse endPoint, int limit, string[] orderByParam, OrderByType order);
        IEnumerable<Route> GetRouteByTime(Warehouse startPoint, Warehouse endPoint, OrderByType orderType);
        IEnumerable<Route> GetRouteByCost(Warehouse startPoint, Warehouse endPoint, OrderByType orderType);
        IEnumerable<Route> GetQuickestRoute(Warehouse startPoint, Warehouse endPoint);
        IEnumerable<Route> GetSlownessRoute(Warehouse startPoint, Warehouse endPoint);
        IEnumerable<Route> GetCheapestRoute(Warehouse startPoint, Warehouse endPoint);
        IEnumerable<Route> GetShortestRoute(Warehouse startPoint, Warehouse endPoint);

    }
}
