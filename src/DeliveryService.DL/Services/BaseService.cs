using DeliveryService.DAL.Models;
using DeliveryService.DL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.DL.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
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
