using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DL.Models
{
    public class RouteOptionsModel
    {
        public int Limit { get;  }
        public string[] OrderByParams { get;  }
        public OrderByType OrderBy { get; }

        public RouteOptionsModel(int limit,string[] orderByParams, bool orderByDescending)
        {
            Limit = limit;
            OrderByParams = orderByParams;
            OrderBy = orderByDescending ? OrderByType.Descending : OrderByType.Ascending;
        }
    }
}
