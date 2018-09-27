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
        IEnumerable<WarehouseResponse> GetAsync();

        WarehouseResponse GetById(int id);

        IEnumerable<WarehouseResponse> Where(Expression<Func<WarehouseResponse, bool>> exp);

        void Add(WarehouseResponse entry);

        void Update(WarehouseResponse entry);

        void Remove(int id);
    }
}
