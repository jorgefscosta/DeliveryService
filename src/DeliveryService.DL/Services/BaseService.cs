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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(T entry)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }
    }
}
