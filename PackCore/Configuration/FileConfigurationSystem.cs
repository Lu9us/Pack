using System;
using System.Collections.Generic;
using System.Text;

namespace PackCore.Configuration
{
   public abstract class FileConfigurationSystem : IConfigurationSystem
    {
        protected Dictionary<string, object> data = new Dictionary<string, object>();

        public object getConfigurationValue(string value)
        {
            if (data.ContainsKey(value)) {
                return value;
            }
            return null;
        }

        public void setConfigurationValue(string key, string value)
        {
            if (data.ContainsKey(key))
            {
                data.Remove(key);
                data.Add(key, value);
            }
            else {
                data.Add(key, value);
            }
        }

       public abstract void readFile(string fileUri);
    }
}
