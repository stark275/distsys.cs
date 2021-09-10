using System;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace Middleware
{
    class Middleware
    {          
        public static void Main(string[] args)
        {
            Server.Server middleware = Server.Server.Factory("localhost", 8002);
            middleware.Start();
           
        }
    }
}