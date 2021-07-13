using System;
using System.Collections.Generic;
using System.Text;
using Pack.src.framework.context;
using Pack.src.framework.eventData;
using Pack.src.framework.http;

namespace Pack.src.framework.verticle
{
    public abstract class AbstractVerticle
    {
        protected IPackContext context;
        protected Guid id;
        protected String name;
        protected AbstractVerticle() {
            id = Guid.NewGuid();
        }

        public String getAddress() {
            if (name != null) {
                return name;
            }
            else {
                return id.ToString();
            }
        }

        public Guid getId() {
          return id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            if (this.name == null) {
                this.name = name;
            }
        }

        internal void setup(IPackContext wolftex) {
            context = wolftex;
        }
        public abstract void Start();
        public void Stop() { }
        public abstract void ReciveMessage(Message message);
        public abstract void ProcessHTTPRequest(HTTPRequest request, HTTPResponse response);

        protected void SendMessage(Message message, String verticle) {
            String messageSender = this.name != null ? this.name : this.id.ToString();
            message.senderId = messageSender;
            context.EnqueEvent(new MessageEvent(messageSender, verticle, message));
        }

    }
}
