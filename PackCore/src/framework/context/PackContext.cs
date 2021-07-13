using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using Pack.src.framework.eventData;
using Pack.src.framework.verticle;
using System.Threading;
using Pack.src.framework.http;
using PackCore.src.framework.clustering;

namespace Pack.src.framework.context
{
    public class PackContext : IPackContext
    {

        ConcurrentDictionary<String, AbstractVerticle> verticles = new ConcurrentDictionary<string, AbstractVerticle>();
        ConcurrentQueue<Event> eventQueue = new ConcurrentQueue<Event>();
        List<EventWorker> workers = new List<EventWorker>();
        HTTPHandler httpHandler;
        Thread eventBus;
        ClusteringVerticle clusterVerticle;
        bool exit = false;
        public PackContext(int workerCount)
        {
            for (int i = 0; i < workerCount; i++) {
                EventWorker worker = new EventWorker();
                worker.Start(this);
                workers.Add(worker);
            }
            eventBus = new Thread(ProcessEventBus);
            eventBus.Start();
        }

        public void createHTTPHandler(int port) {
            httpHandler = new HTTPHandler(this, port);
        }

        public IHTTPHandler getHTTPHandler() {
            return httpHandler;
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
            eventQueue.Enqueue(newEvent);
        }

        public void ExecuteEvent(Event newEvent)
        {
            throw new NotImplementedException();
        }

        public void RegisterVerticle(AbstractVerticle verticle)
        {
            this.verticles.TryAdd(verticle.getId().ToString(), verticle);
            verticle.setup(this);
        }

        public void RegisterVerticle(AbstractVerticle verticle, String name)
        {
            this.verticles.TryAdd(name, verticle);
            verticle.setup(this);
            verticle.setName(name);
            if (clusterVerticle != null) {
                clusterVerticle.SendUpdate();
            }
        }

        private bool WorkerAvalible() {
            foreach (EventWorker worker in workers) {
                if (worker.GetEvent() == null)
                {
                    return true;
                }
            }
            return false;
        }

        private EventWorker getIdleWorker()
        {
            foreach (EventWorker worker in workers)
            {
                if (worker.GetEvent() == null)
                {
                    return worker;
                }
            }
            return null;
        }

        private void ProcessEventBus() {
            while (!exit) {
                if (!eventQueue.IsEmpty && WorkerAvalible()) {
                    EventWorker worker = getIdleWorker();
                    Event eventData;
                    eventQueue.TryDequeue(out eventData);
                    if (eventData != null)
                    {
                        if (verticles.ContainsKey(eventData.target))
                        {
                            worker.PutEvent(eventData);
                            worker.threadHandler.Set();
                        }
                        else if(
                            clusterVerticle != null
                            && clusterVerticle.HasRemoteVerticle(eventData.target) 
                            && eventData is MessageEvent
                            ) {
                            clusterVerticle.SendMessage(eventData as MessageEvent);
                        }
                    }
                }
            }
        }

        public void EnableClustering()
        {
            if (httpHandler != null) {
                clusterVerticle = new ClusteringVerticle(httpHandler.Port);
                this.RegisterVerticle(clusterVerticle);
                httpHandler.RegisterEndpoint(clusterVerticle.MESSAGE_ENDPOINT, "POST", clusterVerticle);
                httpHandler.RegisterEndpoint(clusterVerticle.REGISTER_ENDPOINT, "POST", clusterVerticle);
                httpHandler.RegisterEndpoint(clusterVerticle.UPDATE_ENDPOINT, "POST", clusterVerticle);
            }
        }

        public void ConnectToContext(string contextAddress)
        {
            clusterVerticle.SendRegistration(contextAddress);
        }

        public List<string> getVerticles()
        {
            return new List<string>(this.verticles.Keys);
        }
    }
}
