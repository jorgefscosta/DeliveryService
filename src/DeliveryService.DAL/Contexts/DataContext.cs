using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DAL.Contexts
{
    public class DataContext
    {
        public IGraphClient GraphDb { get; }
        public DataContext(string dbUri, string user, string pass)
        {
            GraphDb = new GraphClient(new Uri(dbUri), user, pass);
            GraphDb.Connect();
        }
    }
}
