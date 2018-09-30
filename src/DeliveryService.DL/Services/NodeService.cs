using DeliveryService.DAL.Models;
using DeliveryService.DL.Repositories;
using System.Collections.Generic;

namespace DeliveryService.DL.Services
{
    public class NodeService<T> : INodeService<T> where T : NodeEntity
    {
        private readonly INodeRepository<T> _repository;

        public NodeService(INodeRepository<T> repository)
        {
            _repository = repository;
        }

        public void Add(T entry)
        {
            _repository.Insert(entry);
        }

        public IEnumerable<T> GetAsync()
        {
            return _repository.GetAll();
        }

        public T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Remove(int id)
        {
            _repository.Delete(id);
        }

        public void Update(T entry)
        {
            _repository.Update(entry);
        }
    }
}
