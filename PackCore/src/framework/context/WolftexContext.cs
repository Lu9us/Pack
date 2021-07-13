using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using Pack.src.framework.eventData;
using Pack.src.framework.verticle;
using System.Threading;

namespace Pack.src.framework.context
{
    public class WolftexContext : IWolftexContext
    {

        ConcurrentDictionary<String, AbstractVerticle> verticles = new ConcurrentDictionary<string, AbstractVerticle>();
        ConcurrentQueue<Event> eventQueue = new ConcurrentQueue<Event>();
        List<EventWorker> workers = new List<EventWorker>();
        Thread eventBus;
        bool exit = false;
        public WolftexContext(int workerCount)
        {
            for (int i = 0; i < workerCount; i++) {
                EventWorker worker = new EventWorker();
                worker.Start(this);
                workers.Add(worker);
            }
            eventBus = new Thread(ProcessEventBus);
            eventBus.Start();
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
                        worker.PutEvent(eventData);
                    }
                }
            }
        }
    }
}
