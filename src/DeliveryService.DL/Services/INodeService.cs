using DeliveryService.DAL.Models;
using System.Collections.Generic;

namespace DeliveryService.DL.Services
{
    public interface INodeService<T> where T : NodeEntity
    {

        IEnumerable<T> GetAsync();

        T GetById(int id);

        void Add(T entry);

        void Update(T entry);

        void Remove(int id);
    }
}
