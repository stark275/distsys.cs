using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Middleware
{
    class NodeManager
    {
        private static readonly HttpClient client = new HttpClient();

        public static Node[] nodes = {
            Node.CrateInstance("http","localhost",5000,"n2"),
            Node.CrateInstance("http","localhost",5000,"n3"),
            Node.CrateInstance("http","localhost",5000,""),
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

    }
}
