using DeliveryService.DAL.Models;
using DeliveryService.DAL.Models.Relationships;
using System.Collections.Generic;

namespace DeliveryService.DL.Repositories
{
    public interface IRelationshipRepository<T, TLeft, TRight>
         where T : RelationshipEntity where TLeft : NodeEntity where TRight : NodeEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetById(int originId, int destinyId);
        void Insert(T entity, TLeft originNode, TRight destinyNode);
        void Update(T entity, TLeft originNode, TRight destinyNode);
        void Delete(T entity, TLeft originNode, TRight destinyNode);
    }
}
