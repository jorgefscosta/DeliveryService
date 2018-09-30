using DeliveryService.DL.Models;
using System.Collections.Generic;

namespace DeliveryService.DL.Services
{
    public interface IWarehouseService
    {
        IEnumerable<WarehouseResponse> Get();

        WarehouseResponse GetById(int id);

        IEnumerable<WarehouseResponse> GetByName(string name);

        void Add(WarehouseResponse entry);

        void Update(WarehouseResponse entry);

        void Remove(int id);
    }
}
