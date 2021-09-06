using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware.Node
{
    class Node
    {

        public string protocol;
        public string host;
        public int port;
        public string path;

        public string type;
        public string state;


        public Node(string protocol, string host, int port, string path)
        {
            this.protocol = protocol;
            this.host = host;
            this.port = port;
            this.path = path;
            type = "server";
            state = "unknown";
        }

        public string GetUrl()
        {
            return protocol + "://" + host + ":" + port + "/" + path;
        }

        public Uri GetUri()
        {
            UriBuilder Builder = new UriBuilder(protocol, host, port, path);
            return Builder.Uri;
        }

        public static Node Factory(string protocol, string host, int port, string path)
        {
            return new Node(protocol, host, port, path);
        }
    }
}
