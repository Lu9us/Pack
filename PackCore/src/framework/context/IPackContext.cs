using System;
using System.Collections.Generic;
using System.Text;
using Pack.src.framework.eventData;
using Pack.src.framework.verticle;

namespace Pack.src.framework.context
{
   public interface IPackContext
    {

        AbstractVerticle GetVerticle(String id);
        void RegisterVerticle(AbstractVerticle verticle);
        void RegisterVerticle(AbstractVerticle verticle, String name);
        void DeregisterVerticle(AbstractVerticle verticle);
        void EnqueEvent(Event newEvent);
        void ExecuteEvent(Event newEvent);
        void EnableClustering();
        void ConnectToContext(String contextAddress);
        List<string> GetVerticles();
    }
}
