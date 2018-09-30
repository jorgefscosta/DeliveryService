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
        IEnumerable<ShipsToResponse> Get();
        IEnumerable<RouteResponse> Get(string originName, string destinyName);
        IEnumerable<RouteResponse> Get(string originName, string destinyName, RouteOptionsModel options);
        IEnumerable<ShipsToResponse> GetDirectRoute(int originId, int destinyId);
        IEnumerable<RouteResponse> GetRoutesFrom(string originName);
        IEnumerable<RouteResponse> GetRoutesTo(string destinyName);
        RouteResponse GetBestRoute(string originName, string destinyName);
        RouteResponse GetQuickestRoute(string originName, string destinyName);
        RouteResponse GetSlownessRoute(string originName, string destinyName);
        RouteResponse GetCheapestRoute(string originName, string destinyName);
        RouteResponse GetCostliestRoute(string originName, string destinyName);
        RouteResponse GetShortestRoute(string originName, string destinyName);

        void Create(ShipsToRequest entity);
        void Update(ShipsToRequest entity);
        void Remove(ShipsToRequest entity);


    }
}
