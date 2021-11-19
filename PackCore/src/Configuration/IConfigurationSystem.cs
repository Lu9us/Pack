using System;
using System.Collections.Generic;
using System.Text;

namespace PackCore.src.Configuration
{
   public interface IConfigurationSystem
    {
        object getConfigurationValue(string value);
        void setConfigurationValue(string key, string value);
    }
}
