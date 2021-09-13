using Middleware.Node.Event;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Node
{
    class NodeManager
    {
        private readonly HttpClient client = new HttpClient();

        private  int currentNode = 0;

       
        public  Node[] nodes = {
            Node.Factory("http","localhost",4000,""),
            Node.Factory("http","localhost",4001,""),
            Node.Factory("http","localhost",4002,""),
        };

        public NodeManager()
        {

        }

        public static NodeManager factory()
        {
            return new NodeManager();
        }

        public  async void PingNode(int nodeId)
        {
            var responseString = "";
            try
            {
                responseString = await this.client.GetStringAsync(nodes[nodeId].GetUri());

                if (responseString.GetType().Equals(typeof(string)))
                {
                    this.nodes[nodeId].state = "Alive";
                }
            }
            catch (Exception)
            {
                nodes[nodeId].state = "down";
                // throw;
            }
           // Console.WriteLine(responseString);
        }

        

        public void PingLoop(int interval)
        {
            _ = SetInterval(() =>
            {
                this.PingNode(this.currentNode);
                if (this.currentNode == nodes.Length - 1)
                    this.currentNode = 0;
                else
                    this.currentNode++;

                foreach (Node node in this.nodes)
                {
                   // Console.WriteLine(node.state);       
                }

                AliveNodesUpdatedEventArgs args = new AliveNodesUpdatedEventArgs();
                args.aliveNodeUrls = this.GetAliveNodeUri();

                this.OnAliveNodesUpdated(args);

                Console.WriteLine("------------------");
                Console.WriteLine();

            }, TimeSpan.FromSeconds(interval));
        }

        private List<Uri> GetAliveNodeUri()
        {
           List<Uri> aliveNodeUriList = new List<Uri>();
            foreach (Node node in nodes)
            {
                if (node.state.Equals("Alive"))
                {
                    aliveNodeUriList.Add(node.GetUri());
                }
            }
            return aliveNodeUriList;
        }

        public  async Task SetInterval(Action action, TimeSpan timeout)
        {
            await Task.Delay(timeout).ConfigureAwait(false);
            action();

            _ = SetInterval(action, timeout);
        }

        protected virtual void OnAliveNodesUpdated(AliveNodesUpdatedEventArgs e)
        {
            EventHandler<AliveNodesUpdatedEventArgs> handler = this.aliveNodesUpdated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<AliveNodesUpdatedEventArgs> aliveNodesUpdated;

    }
}
