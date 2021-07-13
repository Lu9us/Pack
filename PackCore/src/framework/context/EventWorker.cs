using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Pack.src.framework.eventData;
using Pack.src.framework.verticle;

namespace Pack.src.framework.context
{
   public class EventWorker
    {
        IWolftexContext context;
        Thread thread;
        bool exit = false;
        Guid guid = Guid.NewGuid();
        Event currentEvent;

        public void Start(WolftexContext context) {
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
                    if (currentEvent is MesssageEvent)
                    {
                        MesssageEvent mevent = currentEvent as MesssageEvent;
                        vert.ReciveMessage(mevent.message);
                        currentEvent = null;
                    }
                    else if (currentEvent is HttpEvent) {
                        HttpEvent mevent = currentEvent as HttpEvent;
                        vert.ProcessHTTPRequest(mevent.request, mevent.response);
                        currentEvent = null;
                    }
                }
            }
        }
    }
}
