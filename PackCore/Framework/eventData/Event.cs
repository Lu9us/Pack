using System;
using System.Collections.Generic;
using System.Text;

namespace Pack.framework.eventData
{
   public abstract class Event
    {
        public Event(String source, String target) {
            this.source = source;
            this.target = target;
        }
      public readonly String source;
      public readonly String target;
    }
}
