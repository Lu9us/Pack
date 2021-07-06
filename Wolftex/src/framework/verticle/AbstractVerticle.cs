using System;
using System.Collections.Generic;
using System.Text;
using Wolftex.src.framework.context;
using Wolftex.src.framework.http;

namespace Wolftex.src.framework.verticle
{
    public abstract class AbstractVerticle
    {
        protected IWolftexContext context;
        protected String name;
        protected Guid id;
        internal void setup(IWolftexContext wolftex) {
            context = wolftex;
        }
        public abstract void Start();
        public void Stop() { }
        internal void ReciveMessage(Message message) { }
        internal void ProcessHTTPRequest(HTTPRequest request, HTTPResponse response) { }

        internal void SendMessage(Message message, String verticle) { 
        
        }

    }
}
