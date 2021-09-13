using Middleware.Node.Event;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Client
{
    class RequestManager
    {
        public  List<Message> requestTable;
        private  Message message;
        private List<Uri> aliveNodes;
        private readonly HttpClient client;
        private int currentNode;


        public RequestManager()
        {
            this.requestTable = new List<Message>();
            this.client = new HttpClient();
            this.currentNode = 0;         
        }

        public async Task<string> ForwardAsync(Message message)
        {          
            this.requestTable.Add(message);
            string url = message.req.RawUrl.Substring(1);

            if (url.Equals("favicon.ico"))
            {
                return "";
            }

            Uri node = this.aliveNodes[this.currentNode];
            url = node + url;

            this.RandomPolicy();

            Console.WriteLine(url);

            return await client.GetStringAsync(url);

            //Console.WriteLine(message.req.Url);
            //Console.WriteLine(message.req.InputStream);
            //Console.WriteLine(message.req.Headers);

        }

        public void DefineAliveNodes(List<Uri> aliveNodes)
        {
            this.aliveNodes = aliveNodes;
        }

        private void RoundRobinPolicy()
        {
            this.currentNode = (this.currentNode == this.aliveNodes.Count - 1)
                              ? this.currentNode = 0
                              : this.currentNode++;
        }

        private void RandomPolicy()
        {
            this.currentNode = (new Random()).Next(0, this.aliveNodes.Count);
        }


        public void e_aliveNodesUpdated(object sender, AliveNodesUpdatedEventArgs e)
        {
            this.aliveNodes = e.aliveNodeUrls;

            if (e.aliveNodeUrls.Count > 0)
            {
               // Console.WriteLine(e.aliveNodeUrls[0]);
            }

           // Console.WriteLine(this.aliveNodes.Count);

        }
    }
}
