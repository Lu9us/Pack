using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Pack.framework.eventData;
using Pack.framework.verticle;

namespace Pack.framework.context
{
   internal class EventWorker
    {
        IPackContext context;
        Thread thread;
        bool exit = false;
        Guid guid = Guid.NewGuid();
        Event currentEvent;
        internal AutoResetEvent threadHandler = new AutoResetEvent(false);

        public void Start(PackContext context) {
            this.context = context;
            thread = new Thread(ThreadRunner);
            thread.Name = guid.ToString();
            thread.Start();
        }

        public Event GetEvent() {
            return currentEvent;
        }

        public void PutEvent(Event eventData) {
            
            lock (this)
            {
                if (this.currentEvent == null)
                {

                    this.currentEvent = eventData;
                }
            }
        }

        public void ThreadRunner() {
            while (!exit) {
                if (currentEvent != null) {
                    System.Console.WriteLine("Processing event on thread: " + guid.ToString());
                    AbstractVerticle vert = context.GetVerticle(currentEvent.target);
                    if (currentEvent is MessageEvent)
                    {
                        MessageEvent mevent = currentEvent as MessageEvent;
                        vert.ReciveMessage(mevent.message);
                        currentEvent = null;
                    }
                    else if (currentEvent is HttpEvent) {
                        HttpEvent httpEvent = currentEvent as HttpEvent;
                        vert.ProcessHTTPRequest(httpEvent.request, httpEvent.response);
                        currentEvent = null;
                    }
                }
                threadHandler.WaitOne();
            }
        }
    }
}
