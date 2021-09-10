using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Node
{
    class NodeManager
    {
        private static readonly HttpClient client = new HttpClient();

        private static int currentNode = 0;


        public static Node[] nodes = {
            Node.Factory("http","localhost",4000,"n2"),
            Node.Factory("http","localhost",4001,"n3"),
            Node.Factory("http","localhost",4002,""),
        };

        public static async void PingNode(int nodeId)
        {
            var responseString = "";
            try
            {
                responseString = await client.GetStringAsync(nodes[nodeId].GetUri());

                if (responseString.GetType().Equals(typeof(string)))
                {
                    nodes[nodeId].state = "Alive";
                }
            }
            catch (Exception)
            {
                nodes[nodeId].state = "down";
                // throw;
            }
            Console.WriteLine(responseString);
        }

        public static void PingLoop(int interval)
        {
            _ = SetInterval(() =>
            {
                PingNode(currentNode);
                if (currentNode == nodes.Length - 1)
                    currentNode = 0;
                else
                    currentNode++;

                foreach (var node in nodes)
                {
                    Console.WriteLine(node.state);
                }

                Console.WriteLine("------------------");
                Console.WriteLine();

            }, TimeSpan.FromSeconds(interval));
        }

        public static async Task SetInterval(Action action, TimeSpan timeout)
        {
            await Task.Delay(timeout).ConfigureAwait(false);
            action();

            _ = SetInterval(action, timeout);
        }

    }
}
