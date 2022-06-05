using System;
using System.Collections.Generic;
using System.Text;
using Pack.framework.context;
using Pack.framework.eventData;
using Pack.framework.http;

namespace Pack.framework.verticle
{
    public abstract class AbstractVerticle
    {
        protected IPackContext context;
        protected Guid id;
        protected String name;
        protected AbstractVerticle() {
            id = Guid.NewGuid();
        }

        public String GetAddress() {
            if (name != null) {
                return name;
            }
            else {
                return id.ToString();
            }
        }

        public Guid GetId() {
          return id;
        }

        public String GetName() {
            return name;
        }

        public void SetName(String name) {
            if (this.name == null) {
                this.name = name;
            }
        }

        internal void Setup(IPackContext wolftex) {
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
