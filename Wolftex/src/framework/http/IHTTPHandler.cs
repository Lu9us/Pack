using System;
using System.Collections.Generic;
using System.Text;

namespace Wolftex.src.framework.http
{
    public interface IHTTPHandler
    {
       void HandleRequest();
        void SendResponse();
    }
}
