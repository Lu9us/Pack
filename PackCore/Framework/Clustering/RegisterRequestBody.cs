using System;
using System.Collections.Generic;
using System.Text;

namespace PackCore.framework.clustering
{
   internal class RegisterRequestBody
    {
        internal RegisterRequestBody() { }
        internal RegisterRequestBody(string address, List<string> verticles) {
            this.address = address;
            this.verticles = verticles;
        }
       public string address;
       public List<string> verticles = new List<string>();

    }
}
