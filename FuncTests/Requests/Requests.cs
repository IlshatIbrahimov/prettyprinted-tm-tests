using System;
using System.Collections.Generic;
using System.Text;

namespace FuncTests.Requests
{
    public class Requests
    {
        protected BackendClient Client { get; private set; }

        public Requests()
        {
            Client = new BackendClient();
        }
    }
}
