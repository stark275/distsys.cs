using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Middleware.Client
{
    class RequestManager
    {
        public  List<Message> requestTable;
        private  Message message;

        public RequestManager()
        {
            requestTable = new List<Message>();
            
        }

        private void HandleRequest(Message message)
        {
            this.requestTable.Add(message);
        }

        private void ForwardResques()
        {

        }
    }
}
