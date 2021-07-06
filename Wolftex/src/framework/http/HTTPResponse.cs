using System;
using System.Collections.Generic;
using System.Text;

namespace Wolftex.src.framework.http
{
   public class HTTPResponse
    {
        internal HTTPResponse(HTTPHandler handler) {
            this.handler = handler;
        }
        internal HTTPHandler handler;
        public  String uri;
        public  String body;
        public  String verb;
        public  Dictionary<string, string> headers = new Dictionary<string, string>();
        public String statusCode;
        public void End() { }

    }
}
