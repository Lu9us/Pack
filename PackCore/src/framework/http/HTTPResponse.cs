using System;
using System.Collections.Generic;
using System.Text;

namespace Pack.src.framework.http
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
        public  String contentType;
        public  Dictionary<string, string> headers = new Dictionary<string, string>();
        public int statusCode;
        internal delegate void SenderDelegate();
        internal SenderDelegate senderDelegate;
        private bool ended = false;
        public void End()
        {
            if (ended != true)
            {
                senderDelegate.Invoke();
                ended = true;
            }
        }
    }
}
