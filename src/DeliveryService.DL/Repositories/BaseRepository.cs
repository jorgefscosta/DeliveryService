using DeliveryService.DAL.Contexts;
using DeliveryService.DAL.Models;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private string GetCypherNodeMap(string varName)
        {
            return string.Format("({0}:{1})", varName, typeof(T).Name);
        }

        private string GetCypherNodeMapWithParam(string varName, string param)
        {
            return string.Format("({0}:{1} {{2}})", varName, typeof(T).Name,param);
        }

        private ICypherFluentQuery BaseQuery(string varName)
        {
            return _context.GraphDb.Cypher
                    .Match(GetCypherNodeMap(varName));
        }

        private ICypherFluentQuery BaseQueryFilteredById(string varName, int id)
        {
            return _context.GraphDb.Cypher
                    .Match(GetCypherNodeMap(varName))
                    .Where((T x) => x.Id == id);
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
           _context.GraphDb.Cypher
                        .Create(GetCypherNodeMapWithParam("x", "y"))
                        .WithParam("y", entity)
                        .ExecuteWithoutResults();
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
            BaseQueryFilteredById("x", id)
            .Delete("x")
            .ExecuteWithoutResults();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> exp)
        {
            return BaseQuery("x").Where(exp).Return<T>("x").Results;            
        }
    }
}
