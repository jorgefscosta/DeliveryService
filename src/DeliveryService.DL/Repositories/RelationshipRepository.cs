using DeliveryService.DAL.Contexts;
using System.Collections.Generic;
using DeliveryService.DAL.Models.Relationships;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Helpers;
using Neo4jClient.Cypher;

namespace DeliveryService.DL.Repositories
{
    public class RelationshipRepository<T, TLeft, TRight> : IRelationshipRepository<T,TLeft,TRight>
        where T : RelationshipEntity where TLeft : NodeEntity where TRight : NodeEntity
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

        public void Insert(T entity, TLeft originNode, TRight destinyNode)
        {
            var origin = CypherQueries.CypherNodeMap<TLeft>("from");
            var destiny = CypherQueries.CypherNodeMap<TRight>("to");
            _context.GraphDb.Cypher
                .Match(origin, destiny)
                .Where((TLeft from) => from.Id == originNode.Id)
                .AndWhere((TRight to) => to.Id == destinyNode.Id)
                .CreateUnique(string.Format("(from)-[:{0} {{{1}}}]->(to)", typeof(T).Name, "params"))
                .WithParam("params", entity)
                .ExecuteWithoutResults();
        }

        private ICypherFluentQuery BaseRelationshipMap(T entity, TLeft originNode, TRight destinyNode, string relVarName)
        {
            var o = CypherQueries.CypherNodeMap<TLeft>("from");
            var d = CypherQueries.CypherNodeMap<TRight>("to");
            string inputString = string.Format("{0}-[{2}: {3}]->{1}",o, d, relVarName, typeof(T).Name);
            return _context.GraphDb.Cypher
                .Match(inputString)
                .Where((TLeft from) => from.Id == originNode.Id)
                .AndWhere((TRight to) => to.Id == destinyNode.Id);
        }

        public void Update(T entity, TLeft originNode, TRight destinyNode)
        {
            BaseRelationshipMap(entity,originNode,destinyNode, "r")
                .Set("r = {params}")
                .WithParam("params", entity)
                .ExecuteWithoutResults();
        }


        public void Delete(T entity, TLeft originNode, TRight destinyNode)
        {
            BaseRelationshipMap(entity,originNode,destinyNode, "r")
                .Delete("r")
                .ExecuteWithoutResults();
        }
    }
}
