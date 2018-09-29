using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryService.API.Models.Warehouses;
using DeliveryService.DL.Infrastructure;
using DeliveryService.DL.Models;
using DeliveryService.DL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryService.API.Controllers
{

    [Route("api/[controller]")]
    public class WarehousesController : Controller
    {
        private readonly IWarehouseService _service;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public WarehousesController(IWarehouseService service, IMapper mapper, IErrorHandler errorHandler)
        {
            _service = service;
            _mapper = mapper;
            _errorHandler = errorHandler;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<WarehouseResponse> Get()
        {
            return _service.GetAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public WarehouseResponse Get([Required]int id)
        {
            var result = _service.GetById(id);
            if (result == null)
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNotFound));
            }
            return result;
        }

        // GET api/<controller>/5
        [HttpGet("name/{name}")]
        public IEnumerable<WarehouseResponse> GetByName([Required]string name)
        {
            return _service.GetByName(name);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]WarehouseRequestModel entity)
        {
            if(entity==null)
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNull));
            }

            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(_errorHandler.GetErrorMessage(ErrorMessagesEnum.ModelValidation), ModelState.Values.First().Errors.First().ErrorMessage));
            }
            if (_service.GetById(entity.Id) != null)
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityDuplicate));
            }
            var request = _mapper.Map<WarehouseRequestModel, WarehouseResponse>(entity);
            _service.Add(request);
        }

        // PUT api/<controller>
        [HttpPut]
        public void Put([FromBody]WarehouseRequestModel entity)
        {
            if (entity == null)
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNull));
            }

            if (!ModelState.IsValid)
            {
                throw new HttpRequestException(string.Format(_errorHandler.GetErrorMessage(ErrorMessagesEnum.ModelValidation), ModelState.Values.First().Errors.First().ErrorMessage));
            }

            //Not idempotent 
            if (_service.GetById(entity.Id) == null)
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNotFound));
            }

            var request = _mapper.Map<WarehouseRequestModel, WarehouseResponse>(entity);
            _service.Update(request);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete([Required]int id)
        {
            if (_service.GetById(id) == null)
            {
                throw new HttpRequestException(_errorHandler.GetErrorMessage(ErrorMessagesEnum.EntityNotFound));
            }
            _service.Remove(id);
        }
    }
}
