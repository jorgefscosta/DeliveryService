using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.DL.Models;
using DeliveryService.DL.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryService.API.Controllers
{

    [Route("api/[controller]")]
    public class WarehousesController : Controller
    {
         private readonly IWarehouseService _service;
        public WarehousesController(IWarehouseService service)
        {
            _service = service;
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
            return _service.GetById(id);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]WarehouseResponse value)
        {
            _service.Add(value);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]WarehouseResponse value)
        {
            if (Get(id) != null)
            {
                _service.Update(value);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete([Required]int id)
        {
            _service.Remove(id);
        }
    }
}
