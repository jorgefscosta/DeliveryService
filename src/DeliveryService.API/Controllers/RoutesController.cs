using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using AutoMapper;
using DeliveryService.API.Models.Routes;
using DeliveryService.DL.Helpers;
using DeliveryService.DL.Infrastructure;
using DeliveryService.DL.Models;
using DeliveryService.DL.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _service;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        private readonly IWarehouseService _warehouseService;
        public RoutesController(IRouteService service, IMapper mapper, IErrorHandler errorHandler, IWarehouseService warehouseService)
        {
            _service = service;
            _mapper = mapper;
            _errorHandler = errorHandler;
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public IEnumerable<ShipsToResponse> Get()
        {
            return _service.Get();
        }

        // GET api/<controller>/A
        [HttpGet("{name}")]
        public IEnumerable<RouteResponse> GetRoutesFrom([Required]string name)
        {
            return _service.GetRoutesFrom(name);
        }

        // GET api/<controller>/to/B
        [HttpGet("to/{name}")]
        public IEnumerable<RouteResponse> GetRoutesTo([Required]string name)
        {
            return _service.GetRoutesTo(name);
        }

        // GET api/<controller>/A/to/B?limit=5&orderbyparams=Hops&orderbyparams=TotalTime&orderbyparams=TotalCost&orderBydescending=true
        [HttpGet("{origin}/to/{destiny}")]
        public IEnumerable<RouteResponse> GetRoutes([Required]string origin, [Required] string destiny, [FromHeader]RouteRequestModel entity)
        {
            if (entity == null)
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNull));
            }
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(_errorHandler.GetErrorMessage(ErrorMessagesEnum.ModelValidation), ModelState.Values.First().Errors.First().ErrorMessage));
            }

            if (entity.Limit == 0)
            {
                return _service.Get(origin, destiny);
            }
            else
            {
                var routeOptions = new RouteOptionsModel(entity.Limit, entity.OrderByParams, entity.OrderByDescending);
                return _service.Get(origin, destiny, routeOptions);
            }

        }

        // GET api/<controller>/A/to/B/best
        // GET api/<controller>/A/to/B/short
        // GET api/<controller>/A/to/B/cheap
        // GET api/<controller>/A/to/B/expensive
        // GET api/<controller>/A/to/B/quick
        // GET api/<controller>/A/to/B/slow
        [HttpGet("{origin}/to/{destiny}/{option}")]
        public RouteResponse GetRoute([Required]string origin, [Required] string destiny, [Required] string option)
        {
            switch (option)
            {
                case RouteOptions.BestPath : return _service.GetBestRoute(origin, destiny);
                case RouteOptions.ShortestPath: return _service.GetShortestRoute(origin, destiny);
                case RouteOptions.CheapestPath: return _service.GetCheapestRoute(origin, destiny);
                case RouteOptions.CostliesPath: return _service.GetCostliestRoute(origin, destiny);
                case RouteOptions.QuickestPath: return _service.GetQuickestRoute(origin, destiny);
                case RouteOptions.SlownessPath: return _service.GetSlownessRoute(origin, destiny);
                default: throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.InvalidOption));
            }
        }

        private WarehouseResponse GetWarehouseByName(string name)
        {
            try
            {
                return _warehouseService.GetByName(name).Single();
            }
            catch (Exception)
            {
                throw new HttpRequestException(string.Format("{0}-{1}", name, _errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNotFound)));
            }
        }

        private void ValidateRequest(string origin, string destiny, ShipsToRequestModel entity, out WarehouseResponse originWarehouse, out WarehouseResponse destinyWarehouse )
        {
            if (entity == null)
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNull));
            }
            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(_errorHandler.GetErrorMessage(ErrorMessagesEnum.ModelValidation), ModelState.Values.First().Errors.First().ErrorMessage));
            }
            originWarehouse = GetWarehouseByName(origin);
            destinyWarehouse = GetWarehouseByName(destiny);
        }
        [HttpPost("{origin}/to/{destiny}")]
        public void CreateRoute([Required]string origin, [Required] string destiny, [FromBody] ShipsToRequestModel entity)
        {
            WarehouseResponse originWarehouse, destinyWarehouse;
            ValidateRequest(origin, destiny, entity, out originWarehouse, out destinyWarehouse);
                        
            if (_service.GetDirectRoute(originWarehouse.Id, destinyWarehouse.Id).Any())
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityDuplicate));
            }

            var request = new ShipsToRequest
            {
                Cost = entity.Cost,
                Time = entity.Time,
                Origin = originWarehouse,
                Destiny = destinyWarehouse
            };
            _service.Create(request);
        }

        [HttpPut("{origin}/to/{destiny}")]
        public void UpdateRoute([Required]string origin, [Required] string destiny, [FromBody] ShipsToRequestModel entity)
        {
            WarehouseResponse originWarehouse, destinyWarehouse;
            ValidateRequest(origin, destiny, entity, out originWarehouse, out destinyWarehouse);
            if (!_service.GetDirectRoute(originWarehouse.Id, destinyWarehouse.Id).Any())
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNotFound));
            }

            var request = new ShipsToRequest
            {
                Cost = entity.Cost,
                Time = entity.Time,
                Origin = originWarehouse,
                Destiny = destinyWarehouse
            };
            _service.Update(request);
        }

        [HttpDelete("{origin}/to/{destiny}")]
        public void RemoveRoute([Required]string origin, [Required] string destiny)
        {
            var originWarehouse = GetWarehouseByName(origin);
            var destinyWarehouse = GetWarehouseByName(destiny);
            if (!_service.GetDirectRoute(originWarehouse.Id, destinyWarehouse.Id).Any())
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNotFound));
            }

            var request = new ShipsToRequest
            {
                Origin = originWarehouse,
                Destiny = destinyWarehouse
            };
            _service.Remove(request);
        }


    }
}