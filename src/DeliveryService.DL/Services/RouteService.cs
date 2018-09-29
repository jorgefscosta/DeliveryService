using System;
using System.Collections.Generic;
using System.Text;
using DeliveryService.DAL.Contexts;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Models;
using DeliveryService.DL.Repositories;
using Neo4jClient.Cypher;

namespace DeliveryService.DL.Services
{
    public class RouteService : IRouteService 
    {
        private readonly DataContext _context;
        private string SortByTime { get; }
        private string SortByCost { get; }
        private string SortByLength { get; }

        public RouteService(DataContext context)
        {
            _context = context;
            SortByTime = "TotalTime";
            SortByCost = "TotalCost";
            SortByLength = "Hops";
        }
        public IEnumerable<Route> GetRoutes(Warehouse startPoint, Warehouse endPoint, int limit = 10, string[] orderByParams = null, OrderByType order = OrderByType.Ascending)
        {
            //cypher query
            /*
                MATCH path = ((from:Warehouse)-[:SHIPS_TO*2..20]->(to:Warehouse))
                WHERE from.name='A' and to.name='B'
                WITH path, [x IN nodes(path) WHERE (x:Warehouse) | x] AS routeNodes, 
                [y IN relationships(path) | y] AS shipmentDetail,
                length(path) as hops,
                reduce(accumCost = 0, r IN relationships(path)| accumCost + r.cost) AS totalCost,
                reduce(accumTime = 0, r IN relationships(path)| accumTime + r.time) AS totalTime
                return routeNodes, shipmentDetail, totalCost, totalTime, hops
                order by totalTime,totalCost, hops
                limit 1
            */

            var query = _context.GraphDb.Cypher
                .Match("path=((from:Warehouse)-[:SHIPS_TO*2..20]->(to:Warehouse))")
                .Where((Warehouse from) => from.Name == startPoint.Name)
                .AndWhere((Warehouse to) => to.Name == endPoint.Name)
                .Return(() => new Route
                {
                    StartPoint = Return.As<Warehouse>("from"),
                    EndPoint = Return.As<Warehouse>("to"),
                    RoutePoints = Return.As<IEnumerable<Warehouse>>("[x IN nodes(path) WHERE (x:Warehouse) | x]"),
                    ShipDetails = Return.As<IEnumerable<ShipsTo>>("[y IN relationships(path) | y]"),
                    Hops = Return.As<int>("length(path)"),
                    TotalCost = Return.As<int>("reduce(accumCost = 0, r IN relationships(path)| accumCost + r.cost)"),
                    TotalTime = Return.As<int>("reduce(accumTime = 0, r IN relationships(path)| accumTime + r.time)")
                }).Limit(limit);

            if (orderByParams != null)
                query = order == OrderByType.Ascending ? query.OrderBy(orderByParams) : query.OrderByDescending(orderByParams);

            //Return.As<IEnumerable<Warehouse>>("[x IN nodes(path) WHERE (x:Warehouse) | x]"))
            //.Return(() => Return.As<IEnumerable<Warehouse>>("[x IN nodes(path) WHERE (x:Warehouse) | x]"))
            //.Match("shipmentDetail=[y IN relationships(path) | y]");

            return query.Results;
        }

        public IEnumerable<Route> GetRouteByTime(Warehouse startPoint, Warehouse endPoint, OrderByType orderType)
        {
            var orderParams = new string[] { SortByTime };
            return GetRoutes(startPoint, endPoint, 1, orderParams, orderType);
        }
        public IEnumerable<Route> GetRouteByCost(Warehouse startPoint, Warehouse endPoint, OrderByType orderType)
        {
            var orderParams = new string[] { SortByCost };
            return GetRoutes(startPoint, endPoint, 1, orderParams, orderType);
        }

        public IEnumerable<Route> GetQuickestRoute(Warehouse startPoint, Warehouse endPoint)
        {
            return GetRouteByTime(startPoint, endPoint, OrderByType.Ascending);
        }
        public IEnumerable<Route> GetSlownessRoute(Warehouse startPoint, Warehouse endPoint)
        {
            return GetRouteByTime(startPoint, endPoint, OrderByType.Descending);
        }
        
        public IEnumerable<Route> GetCheapestRoute(Warehouse startPoint, Warehouse endPoint)
        {
            var orderParams = new string[] { SortByCost };
            return GetRoutes(startPoint, endPoint, 1, orderParams, OrderByType.Ascending);
        }
        public IEnumerable<Route> GetShortestRoute(Warehouse startPoint, Warehouse endPoint)
        {
            var orderParams = new string[] { SortByLength };
            return GetRoutes(startPoint, endPoint, 1, orderParams, OrderByType.Ascending);
        }
    }
}
