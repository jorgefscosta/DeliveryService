using DeliveryService.DAL.Contexts;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Helpers;
using Neo4jClient.Cypher;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.DL.Repositories
{
    public class NodeRepository<T> : INodeRepository<T> where T: NodeEntity
    {
        private readonly DataContext _context;

        //TODO: ErrorHandler
        public NodeRepository(DataContext context)
        {
            _context = context;
        }

        private string CypherNodeMap(string varName)
        {
            return string.Format("({0}:{1})", varName, typeof(T).Name);
        }

        private ICypherFluentQuery BaseQuery(string varName)
        {
            return _context.GraphDb.Cypher
                    .Match(CypherNodeMap(varName));
        }

        private ICypherFluentQuery BaseQueryFilteredById(string varName, int id)
        {
            return BaseQuery(varName).
                   Where(CypherQueries.CypherWhereClauseWithIntParam(varName,"Id",id));
        }

        public IEnumerable<T> GetAll()
        {            
            var query = BaseQuery("x")
                        .Return<T>("x");
            return query.Results;
        }

        public T GetById(int id)
        {
            var query = BaseQueryFilteredById("x", id)
                        .Return<T>("x");
            return query.Results.SingleOrDefault();
        }

        public void Insert(T entity)
        {
            var query = _context.GraphDb.Cypher
                          .Create(CypherQueries.CypherNodeMapWithParam<T>("x", "y"))
                          .WithParam("y", entity);
            query.ExecuteWithoutResults();
        }

        public void Update(T entity)
        {
            BaseQueryFilteredById("x", entity.Id)
              .Set("x = {y}")
              .WithParam("y", entity)
              .ExecuteWithoutResults();
        }

        public void Delete(int id)
        {
            var query = _context.GraphDb.Cypher
                        .OptionalMatch(string.Format("{0}-[{1}]-()", CypherNodeMap("x"), "r"))
                        .Where(CypherQueries.CypherWhereClauseWithIntParam("x", "Id", id))
                        .Delete("r, x");
            query.ExecuteWithoutResults();

            //if no relationships exists
            //BaseQueryFilteredById("x", id).Delete("x").ExecuteWithoutResults();
        }
    }
}
