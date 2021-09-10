using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Middleware.Node;

namespace Middleware.Server
{
    class Server
    {
        private readonly int port;
        private  string host;
        private HttpListener httpListener;

        public Server(string url, int port)
        {
            this.port = port;
            this.host = url;
            this.httpListener = new HttpListener();
        }

        public static Server Factory( string host, int port)
        {
            return new Server(host, port);
        }

        private  async Task Listen()
        {
            while (true)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await this.httpListener.GetContextAsync();
                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse res = ctx.Response;


                string customerID = null;
                // Did the request come with a cookie?
                Cookie cookie = req.Cookies["ID"];
                if (cookie != null)
                {
                    customerID = cookie.Value;
                }
                if (customerID != null)
                {
                    Console.WriteLine(req.Cookies["ID"]);
                    Console.WriteLine(res.Cookies["ID"]);

                }
                if (customerID == null)
                {
                    Random rnd = new Random();
                    customerID = rnd.Next().ToString();
                    Cookie cook = new Cookie("ID", customerID);
                    res.AppendCookie(cook);
                }


                byte[] data = Encoding.UTF8.GetBytes(String.Format("your id is {0}", customerID));
                await res.OutputStream.WriteAsync(data, 0, data.Length);

                res.Close();
            }
        }

        public void Start()
        {
            var uri = this.GenerateUri();

            this.httpListener.Prefixes.Add(uri);
            this.httpListener.Start();

            Console.WriteLine("Listening for connections on {0}", this.host);

            NodeManager.PingLoop(2);

            Task listenTask = this.Listen();
            listenTask.GetAwaiter().GetResult();

            this.httpListener.Close();
        }

        private string GenerateUri()
        {
            return "http://" + this.host + ":" + this.port + "/";
        }
    }
}
