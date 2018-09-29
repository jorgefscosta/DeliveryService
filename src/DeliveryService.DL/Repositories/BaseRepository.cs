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

        private string CypherNodeMap(string varName)
        {
            return string.Format("({0}:{1})", varName, typeof(T).Name);
        }

        private string CypherNodeMapWithParam(string varName, string param)
        {
            return string.Format("({0}:{1} {{{2}}})", varName, typeof(T).Name,param);
        }

        private string CypherWhereClauseWithIntParam(string varName, string propName, int paramValue)
        {
            return string.Format("{0}.{1} = {2}", varName,propName, paramValue);
        }
        private string CypherWhereClauseWithStringParam(string varName, string propName, string paramValue)
        {
            return string.Format("{0}.{1} = '{2}'", varName, propName, paramValue);
        }

        private ICypherFluentQuery BaseQuery(string varName)
        {
            return _context.GraphDb.Cypher
                    .Match(CypherNodeMap(varName));
        }

        private ICypherFluentQuery BaseQueryFilteredById(string varName, int id)
        {
            return BaseQuery(varName).
                   Where(CypherWhereClauseWithIntParam(varName,"Id",id));
                    //.Where((T x) => x.Id == id);
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
                          .Create(CypherNodeMapWithParam("x", "y"))
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
                        .Where(CypherWhereClauseWithIntParam("x", "Id", id))
                        .Delete("r, x");
            query.ExecuteWithoutResults();

            //if no relationships exists
            //BaseQueryFilteredById("x", id).Delete("x").ExecuteWithoutResults();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> exp)
        {
            return BaseQuery("x").Where(exp).Return<T>("x").Results;            
        }
    }
}
