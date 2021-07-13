using System;
using System.Collections.Generic;
using System.Text;

namespace Pack.src.framework.http
{
    public class HTTPRequest
    {
      public readonly Dictionary<string, string> headers = new Dictionary<string, string>();

        public HTTPRequest(Dictionary<string, string> headers, String uri, string body, string verb)
        {
            this.headers = headers;
            this.uri = uri;
            this.body = body;
            this.verb = verb;
        }

        public readonly String uri;
        public readonly String body;
        public readonly String verb;
        public List<string> wildCardValues;
    }
}
