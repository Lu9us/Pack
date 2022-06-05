using System;
using System.Collections.Generic;
using System.Text;
using Pack.framework.verticle;

namespace Pack.framework.eventData
{
    public class MessageEvent : Event
    {
        public readonly Message message;
        public MessageEvent(string source, string target, Message message) : base(source, target)
        {
            this.message = message;
        }
    }
}
