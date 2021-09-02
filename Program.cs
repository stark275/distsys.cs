using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace Middleware
{
    class HttpServer
    {       
      
        public static HttpListener listener;

        public static string url = "http://localhost:8002/";

  
        public static async Task Listen()
        {
            while (true)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();
                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;

                string customerID = null;
                // Did the request come with a cookie?
                Cookie cookie = req.Cookies["ID"];
                if (cookie != null)
                {
                    customerID = cookie.Value;
                }

                if (customerID != null)
                {
                    Console.WriteLine("Found the cookie!");
                }

                HttpListenerResponse res = ctx.Response;
                if (customerID == null)
                {
                    Random rnd = new Random();
                    customerID =  rnd.Next().ToString();
                    Cookie cook = new Cookie("ID", customerID);
                    res.AppendCookie(cook);
                }

                byte[] data = Encoding.UTF8.GetBytes(String.Format("your id is {0}",customerID));
                await res.OutputStream.WriteAsync(data, 0, data.Length);

                res.Close();       
            }
        }

       
        public static async Task SetInterval(Action action, TimeSpan timeout)
        {
            await Task.Delay(timeout).ConfigureAwait(false);
            action();

            _ = SetInterval(action, timeout);
        }

        
        public static void Main(string[] args)
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            var i = 0;
            var nodes = NodeManager.nodes;

            _ = SetInterval(() => {
           
                NodeManager.PingNode(i);
          
                if (i == nodes.Length - 1)
                    i = 0;
                else
                    i++;

                foreach (var node in nodes)
                {
                    Console.WriteLine(node.state);
                }

                Console.WriteLine("------------------");
                Console.WriteLine();

            }, TimeSpan.FromSeconds(2));
         
            Task listenTask = Listen();
            listenTask.GetAwaiter().GetResult();

            listener.Close();

        }
    }
}