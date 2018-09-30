using DeliveryService.DAL.Models;
using System.Collections.Generic;

namespace DeliveryService.DL.Repositories
{
    public interface INodeRepository<T> where T : NodeEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
