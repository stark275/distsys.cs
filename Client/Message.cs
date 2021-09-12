using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


namespace Middleware.Client
{
    class Message
    {
        public string id;
        public string clientId;
        public string nodeUrl;
        public HttpListenerRequest req;
        public HttpListenerResponse res;

        public Message(
            string id,
            string clientId,
            string nodeUrl,
            HttpListenerRequest req,
            HttpListenerResponse res)
        {
            this.id = id;
            this.clientId = clientId;
            this.nodeUrl = nodeUrl;
            this.req = req;
            this.res = res;
        }



    }
}
