using DeliveryService.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.DL.Services
{
    public interface IBaseService<T> where T : BaseEntity
    {

        IEnumerable<T> GetAsync();

        T GetById(int id);

        void Add(T entry);

        void Update(T entry);

        void Remove(int id);
    }
}
