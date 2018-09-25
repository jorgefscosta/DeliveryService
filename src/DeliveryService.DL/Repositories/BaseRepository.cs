using DeliveryService.DAL.Contexts;
using DeliveryService.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.DL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T: BaseEntity
    {
        private readonly DataContext _context;

        //TODO: ErrorHandler
        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }
    }
}
