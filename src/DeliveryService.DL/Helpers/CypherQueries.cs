namespace DeliveryService.DL.Helpers
{
    public static class CypherQueries
    {
        public static string CypherNodeMap<T>(string varName)
        {
            return string.Format("({0}:{1})", varName, typeof(T).Name);
        }

        public static string CypherNodeMapWithParam<T>(string varName, string param)
        {
            return string.Format("({0}:{1} {{{2}}})", varName, typeof(T).Name, param);
        }

        public static string CypherWhereClauseWithIntParam(string varName, string propName, int paramValue)
        {
            return string.Format("{0}.{1} = {2}", varName, propName, paramValue);
        }
        public static string CypherWhereClauseWithStringParam(string varName, string propName, string paramValue)
        {
            return string.Format("{0}.{1} = '{2}'", varName, propName, paramValue);
        }
    }
}
