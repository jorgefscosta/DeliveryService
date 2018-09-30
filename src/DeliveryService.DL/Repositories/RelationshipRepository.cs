using DeliveryService.DAL.Contexts;
using System;
using System.Collections.Generic;
using DeliveryService.DAL.Models.Relationships;
using System.Text;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Helpers;
using Neo4jClient.Cypher;
using System.Linq;

namespace DeliveryService.DL.Repositories
{
    public class RelationshipRepository<T, TLeft, TRight> : IRelationshipRepository<T>
        where T : RelationshipEntity where TLeft : BaseEntity where TRight : BaseEntity
    {
        private readonly DataContext _context;

        //TODO: ErrorHandler
        public RelationshipRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            string inputString = string.Format("()-[{0}: {1}]->()", "r", typeof(T).Name);
            var query = _context.GraphDb.Cypher
                .Match(inputString)
                .Return<T>("r");
            return query.Results;
        }

        public IEnumerable<T> GetById(int originId, int destinyId)
        {
            var o = CypherQueries.CypherNodeMap<TLeft>("from");
            var d = CypherQueries.CypherNodeMap<TRight>("to");
            string inputString = string.Format("{0}-[{2}: {3}]->{1}", o, d, "r", typeof(T).Name);
            var query = _context.GraphDb.Cypher
                .Match(inputString)
                .Where((TLeft from) => from.Id == originId)
                .AndWhere((TRight to) => to.Id == destinyId)
                .Return<T>("r");
            return query.Results;
        }

        public void Insert(T entity)
        {
            var origin = CypherQueries.CypherNodeMap<TLeft>("from");
            var destiny = CypherQueries.CypherNodeMap<TRight>("to");
            _context.GraphDb.Cypher
                .Match(origin, destiny)
                .Where((TLeft from) => from.Id == entity.OriginId)
                .AndWhere((TRight to) => to.Id == entity.DestinyId)
                .CreateUnique(string.Format("(from)-[:{0} {{{1}}}]->(to)", typeof(T).Name, "params"))
                .WithParam("params", entity)
                .ExecuteWithoutResults();
        }

        private ICypherFluentQuery BaseRelationshipMap(T entity, string relVarName)
        {
            var o = CypherQueries.CypherNodeMap<TLeft>("from");
            var d = CypherQueries.CypherNodeMap<TRight>("to");
            string inputString = string.Format("{0}-[{2}: {3}]->{1}",o, d, relVarName, typeof(T).Name);
            return _context.GraphDb.Cypher
                .Match(inputString)
                .Where((TLeft from) => from.Id == entity.OriginId)
                .AndWhere((TRight to) => to.Id == entity.DestinyId);
        }

        public void Update(T entity)
        {
            BaseRelationshipMap(entity, "r")
                .Set("r = {params}")
                .WithParam("params", entity)
                .ExecuteWithoutResults();
        }


        public void Delete(T entity)
        {
            BaseRelationshipMap(entity, "r")
                .Delete("r")
                .ExecuteWithoutResults();
        }
    }
}
