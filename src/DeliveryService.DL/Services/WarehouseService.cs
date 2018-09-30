using System.Collections.Generic;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Models;
using AutoMapper;
using System.Linq;

namespace DeliveryService.DL.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly INodeService<Warehouse> _service;
        private readonly IMapper _mapper;

        public WarehouseService(INodeService<Warehouse> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public void Add(WarehouseResponse entry)
        {
            _service.Add(_mapper.Map<WarehouseResponse, Warehouse>(entry));
        }

        public IEnumerable<WarehouseResponse> Get()
        {
            var result = _service.GetAsync();
            return result.Select(t => _mapper.Map<Warehouse, WarehouseResponse>(t));
        }

        public WarehouseResponse GetById(int id)
        {
            var result = _service.GetById(id);
            return _mapper.Map<Warehouse, WarehouseResponse>(result);
        }

        public IEnumerable<WarehouseResponse> GetByName(string name)
        {
            var result = _service.GetAsync().Where(x => x.Name == name);
            return result.Select(t => _mapper.Map<Warehouse, WarehouseResponse>(t));
        }

        public void Remove(int id)
        {
            _service.Remove(id);
        }

        public void Update(WarehouseResponse entry)
        {
            _service.Update(_mapper.Map<WarehouseResponse, Warehouse>(entry));
        }
    }
}
