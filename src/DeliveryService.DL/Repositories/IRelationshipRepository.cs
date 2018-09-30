using DeliveryService.DAL.Models;
using DeliveryService.DAL.Models.Relationships;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DL.Repositories
{
   public interface IRelationshipRepository<T> where T : RelationshipEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetById(int originId, int destinyId);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
