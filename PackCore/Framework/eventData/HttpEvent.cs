using System;
using System.Collections.Generic;
using System.Text;
using Pack.framework.http;

namespace Pack.framework.eventData
{
   public class HttpEvent: Event
    {
       public HTTPRequest request;
       public HTTPResponse response;

        public HttpEvent(HTTPRequest request, HTTPResponse response, String source, String target): base(source, target)
        {
            this.request = request;
            this.response = response;
        }
    }
}
