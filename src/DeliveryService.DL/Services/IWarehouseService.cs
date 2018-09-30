using DeliveryService.DAL.Models;
using DeliveryService.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.DL.Services
{
    public interface IWarehouseService
    {
        IEnumerable<WarehouseResponse> Get();

        WarehouseResponse GetById(int id);

        IEnumerable<WarehouseResponse> GetByName(string name);

        IEnumerable<WarehouseResponse> Where(Expression<Func<WarehouseResponse, bool>> exp);

        void Add(WarehouseResponse entry);

        void Update(WarehouseResponse entry);

        void Remove(int id);
    }
}
