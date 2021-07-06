using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using Wolftex.src.framework.eventData;
using Wolftex.src.framework.verticle;

namespace Wolftex.src.framework.context
{
    public class WolftexContext : IWolftexContext
    {

        ConcurrentDictionary<String, AbstractVerticle> verticles = new ConcurrentDictionary<string, AbstractVerticle>();
        public WolftexContext(int workerCount)
        {

        }

        public AbstractVerticle GetVerticle(String id) {
            AbstractVerticle verticle;
            verticles.TryGetValue(id, out verticle);
            return verticle;
        }
        
        public void DeregisterVerticle(AbstractVerticle verticle)
        {
            throw new NotImplementedException();
        }


        public void EnqueEvent(Event newEvent)
        {
            throw new NotImplementedException();
        }

        public void ExecuteEvent(Event newEvent)
        {
            throw new NotImplementedException();
        }

        public void RegisterVerticle(AbstractVerticle verticle)
        {
            throw new NotImplementedException();
        }

        public void RegisterVerticle(AbstractVerticle verticle, String name)
        {
            throw new NotImplementedException();
        }
    }
}
