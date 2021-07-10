using System;
using System.Collections.Generic;
using System.Text;

namespace Wolftex.src.framework.http
{
   public class HTTPUriContext
    {
       private static String wildCard = "*"; 
       public String path;
       public String verb;
        public bool MatchesQuery(HTTPRequest request) {
            if (request.verb.ToLower() == verb.ToLower())
            {
                String urlParams = request.uri.Split("?").Length > 1 ? request.uri.Split("?")[1] : null;
                String[] queryData = request.uri.Split("?")[0].Split("//");
                String query = queryData[1];
                String requestPath = query.Substring(query.IndexOf("/"));
                String[] splitPath = requestPath.Substring(1).Split("/");
                String[] splitContext = path.StartsWith("/") ? path.Substring(1).Split("/") : path.Split("/");
                if (splitContext.Length == splitPath.Length)
                {
                    for (int i = 0; i < splitContext.Length; i++)
                    {
                        if (splitContext[i] == "*" || splitContext[i] == splitPath[i])
                        {
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;

                }
                else
                {
                    return false;
                }
            }
            return false;
       }
    }
}
