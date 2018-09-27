using DeliveryService.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeliveryService.DL.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Where(Expression<Func<T, bool>> exp);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
