using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Wolftex.src.framework.eventData;
using Wolftex.src.framework.verticle;

namespace Wolftex.src.framework.context
{
   public class EventWorker
    {
        IWolftexContext context;
        Thread thread;
        bool exit = false;
        Event currentEvent;

        public void threadRunner() {
            while (!exit) {
                if (currentEvent != null) {
                    AbstractVerticle vert = context.GetVerticle(currentEvent.target);
                    if (currentEvent is MesssageEvent) {
                        MesssageEvent mevent = currentEvent as MesssageEvent;
                        vert.ReciveMessage(mevent.message); 
                    }
                }
            }
        }
    }
}
