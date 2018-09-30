using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DeliveryService.DAL.Contexts;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Helpers;
using DeliveryService.DL.Models;
using DeliveryService.DL.Repositories;
using Neo4jClient.Cypher;

namespace DeliveryService.DL.Services
{
    public class RouteService : IRouteService
    {
        private readonly DataContext _context;
        private readonly RouteSetup _setup;
        private readonly IRelationshipRepository<SHIPS_TO> _repository;
        private readonly IMapper _mapper;

        public RouteService(DataContext context, RouteSetup setup, IRelationshipRepository<SHIPS_TO> repository, IMapper mapper)
        {
            _context = context;
            _setup = setup;
            _repository = repository;
            _mapper = mapper;
        }
        
        #region Get
        /// <summary>
        /// String format for a cypher relationship where 
        /// "from" is the variable name for the origin Warehouse and
        /// "to" is the variable name for the destiny Warehouse
        /// </summary>
        /// <returns>Returns a relationship between two Warehouses
        /// which "path" is the variable name</returns>
        private ICypherFluentQuery BaseQuery()
        {
            var inputString = string.Format("path=((from:Warehouse)-[:SHIPS_TO*{0}..{1}]->(to:Warehouse))", _setup.MinHops, _setup.MaxHops);
            return _context.GraphDb.Cypher
                .Match(inputString);
        }

        public IEnumerable<ShipsToResponse> GetDirectRoute(int originId, int destinyId)
        {
            var result = _repository.GetById(originId,destinyId);
            return result.Select(t => _mapper.Map<SHIPS_TO, ShipsToResponse>(t));

            //var inputString = "path=((from:Warehouse)-[r:SHIPS_TO]->(to:Warehouse))";
            //return _context.GraphDb.Cypher
            //        .Match(inputString)
            //        .Where((Warehouse from) => from.Name == originName)
            //        .AndWhere((Warehouse to) => to.Name == destinyName)
            //        .Return(() => new ShipsToResponse
            //        {
            //            Cost = Return.As<int>("r.cost"),
            //            Time = Return.As<int>("r.time"),
            //            OriginId = Return.As<int>("from.id"),
            //            DestinyId = Return.As<int>("to.id"),
            //        }).Results;
        }

        private ICypherFluentQuery<RouteResponse> ReturnRouteResponse(ICypherFluentQuery query)
        {
            return query.Return(() => new RouteResponse
            {
                Origin = Return.As<Warehouse>("from"),
                Destiny = Return.As<Warehouse>("to"),
                RoutePoints = Return.As<IEnumerable<Warehouse>>("[x IN nodes(path) WHERE (x:Warehouse) | x]"),
                ShipDetails = Return.As<IEnumerable<SHIPS_TO>>("[y IN relationships(path) | y]"),
                Hops = Return.As<int>("length(path)"),
                TotalCost = Return.As<int>("reduce(accumCost = 0, r IN relationships(path)| accumCost + r.cost)"),
                TotalTime = Return.As<int>("reduce(accumTime = 0, r IN relationships(path)| accumTime + r.time)")
            });
        }
        public IEnumerable<ShipsToResponse> Get()
        {
            var result = _repository.GetAll();
            return result.Select(t => _mapper.Map<SHIPS_TO, ShipsToResponse>(t));
            //return ReturnRouteResponse(BaseQuery()).Results;
        }
        public IEnumerable<RouteResponse> Get(string originName, string destinyName)
        {
            return GetRoutes(originName, destinyName);
        }

        public IEnumerable<RouteResponse> Get(string originName, string destinyName, RouteOptionsModel options)
        {
            return GetRoutes(originName, destinyName, options.Limit, options.OrderByParams, options.OrderBy);
        }
        private IEnumerable<RouteResponse> GetRoutes(string originName, string destinyName, int limit = 10, string[] orderByParams = null, OrderByType order = OrderByType.Ascending)
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
            var query = BaseQuery()
                .Where((Warehouse from) => from.Name == originName)
                .AndWhere((Warehouse to) => to.Name == destinyName);

            var result = ReturnRouteResponse(query);
            if (orderByParams != null)
                result = order == OrderByType.Ascending ? result.OrderBy(orderByParams) : result.OrderByDescending(orderByParams);

            return result.Limit(limit).Results;
        }
        public IEnumerable<RouteResponse> GetRoutesFrom(string originName)
        {
            var query = BaseQuery()
                    .Where((Warehouse from) => from.Name == originName);
            return ReturnRouteResponse(query).Results;
        }
        public IEnumerable<RouteResponse> GetRoutesTo(string destinyName)
        {
            var query = BaseQuery()
                    .Where((Warehouse to) => to.Name == destinyName);
            return ReturnRouteResponse(query).Results;
        }
        public RouteResponse GetBestRoute(string originName, string destinyName)
        {
            var orderParams = new string[] { RouteOptions.SortByTime, RouteOptions.SortByCost, RouteOptions.SortByLength };
            return GetRoutes(originName, destinyName, 1, orderParams, OrderByType.Ascending).FirstOrDefault();
        }
        private RouteResponse GetRouteByTime(string originName, string destinyName, OrderByType orderType)
        {
            var orderParams = new string[] { RouteOptions.SortByTime };
            return GetRoutes(originName, destinyName, 1, orderParams, orderType).FirstOrDefault();
        }
        private RouteResponse GetRouteByCost(string originName, string destinyName, OrderByType orderType)
        {
            var orderParams = new string[] { RouteOptions.SortByCost };
            return GetRoutes(originName, destinyName, 1, orderParams, orderType).FirstOrDefault();
        }

        public RouteResponse GetQuickestRoute(string originName, string destinyName)
        {
            return GetRouteByTime(originName, destinyName, OrderByType.Ascending);
        }
        public RouteResponse GetSlownessRoute(string originName, string destinyName)
        {
            return GetRouteByTime(originName, destinyName, OrderByType.Descending);
        }
        public RouteResponse GetCheapestRoute(string originName, string destinyName)
        {
            return GetRouteByCost(originName, destinyName, OrderByType.Ascending);
        }
        public RouteResponse GetCostliestRoute(string originName, string destinyName)
        {
            return GetRouteByCost(originName, destinyName, OrderByType.Descending);
        }
        public RouteResponse GetShortestRoute(string originName, string destinyName)
        {
            var orderParams = new string[] { RouteOptions.SortByLength };
            return GetRoutes(originName, destinyName, 1, orderParams, OrderByType.Ascending).FirstOrDefault();
        }
        #endregion

        private SHIPS_TO ConvertShipsTo(int time, int cost)
        {
            return new SHIPS_TO
            {
                Cost = cost,
                Time = time,
            };
        }

        public void Create(ShipsToRequest entity)
        {
            //_context.GraphDb.Cypher
            //    .Match("(from:Warehouse)", "(to:Warehouse)")
            //    .Where((Warehouse from) => from.Name == entity.OriginId)
            //    .AndWhere((Warehouse to) => to.Name == entity.DestinyId)
            //    .CreateUnique("(from)-[:SHIPS_TO {params}]->(to)")
            //    .WithParam("params", ConvertShipsTo(entity.Time, entity.Cost))
            //    .ExecuteWithoutResults();
            _repository.Insert(_mapper.Map<ShipsToRequest, SHIPS_TO>(entity));
        }

        public void Update(ShipsToRequest entity)
        {
            //_context.GraphDb.Cypher
            //    .Match("(from:Warehouse)-[r:SHIPS_TO]->(to:Warehouse)")
            //    .Where((Warehouse from) => from.Name == entity.OriginId)
            //    .AndWhere((Warehouse to) => to.Name == entity.DestinyId)
            //    .Set("r = {params}")
            //    .WithParam("params", ConvertShipsTo(entity.Time, entity.Cost))
            //    .ExecuteWithoutResults();
            _repository.Update(_mapper.Map<ShipsToRequest, SHIPS_TO>(entity));
        }
        public void Remove(ShipsToRequest entity)
        {
            //_context.GraphDb.Cypher
            //    .Match("(from:Warehouse)-[r:SHIPS_TO]->(to:Warehouse)")
            //    .Where((Warehouse from) => from.Name == entity.OriginName)
            //    .AndWhere((Warehouse to) => to.Name == entity.DestinyName)
            //    .Delete("r")
            //    .ExecuteWithoutResults();
            _repository.Delete(_mapper.Map<ShipsToRequest, SHIPS_TO>(entity));
        }
    }
}
