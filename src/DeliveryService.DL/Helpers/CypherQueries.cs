using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.DL.Helpers
{
    public static class CypherQueries
    {
        public static string CypherNodeMap<T>(string varName)
        {
            return string.Format("({0}:{1})", varName, typeof(T).Name);
        }
    }
}
