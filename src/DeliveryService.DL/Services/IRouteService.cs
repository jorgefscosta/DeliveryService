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
        IEnumerable<RouteResponse> Get();
        IEnumerable<RouteResponse> Get(string originName, string destinyName);
        IEnumerable<RouteResponse> Get(string originName, string destinyName, RouteOptionsModel options);
        IEnumerable<ShipsToResponse> GetDirectRoute(string originName, string destinyName);
        IEnumerable<RouteResponse> GetRoutesFrom(string originName);
        IEnumerable<RouteResponse> GetRoutesTo(string destinyName);
        RouteResponse GetBestRoute(string originName, string destinyName);
        RouteResponse GetQuickestRoute(string originName, string destinyName);
        RouteResponse GetSlownessRoute(string originName, string destinyName);
        RouteResponse GetCheapestRoute(string originName, string destinyName);
        RouteResponse GetCostliestRoute(string originName, string destinyName);
        RouteResponse GetShortestRoute(string originName, string destinyName);

        void Add(ShipsToResponse entity);
        void Update(ShipsToResponse entity);
        void Delete(ShipsToResponse entity);


    }
}
