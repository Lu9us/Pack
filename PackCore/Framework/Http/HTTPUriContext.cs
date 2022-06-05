using System;
using System.Collections.Generic;
using System.Text;

namespace Pack.framework.http
{
    public class HTTPUriContext
    {
        public HTTPUriContext(String path, String verb)
        {
            this.path = path;
            this.splitPath = path.StartsWith("/") ? path.Substring(1).Split("/") : path.Split("/");
            this.verb = verb;
        }
        private static String wildCard = "*";
        public String path;
        public String[] splitPath;
        public String verb;
        public bool MatchesQuery(String[] requestURI, String requestType)
        {
            if (requestType.ToLower() == verb.ToLower())
            {

                if (this.splitPath.Length == requestURI.Length)
                {
                    for (int i = 0; i < requestURI.Length; i++)
                    {
                        if (requestURI[i] == wildCard || requestURI[i] == splitPath[i])
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


       public List<String> GetWildCardValues(String[] requestURI)
        {
            List<string> wildCardValues = new List<string>();
            if (this.splitPath.Length == requestURI.Length)
            {
                for (int i = 0; i < requestURI.Length; i++)
                {
                    if (requestURI[i] == wildCard)
                    {
                        wildCardValues.Add(requestURI[i]);
                    }
                }
            }
            return wildCardValues;
        }
    }
}
