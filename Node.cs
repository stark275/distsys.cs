using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware
{
    class Node
    {
        
        public string protocol;
        public string host ;
        public int port ;
        public string path;

        public string type;
        public string state;


        public Node(string protocol, string host, int port, string path)
        {
            this.protocol = protocol;
            this.host = host;
            this.port = port;
            this.path = path;
            this.type = "server";
            this.state = "unknown";
        }

        public string GetUrl()
        {
            return this.protocol + "://" + this.host + ":" + this.port + "/" + this.path;
        }

        public Uri GetUri()
        {
            UriBuilder Builder = new UriBuilder(this.protocol, this.host, this.port, this.path);
            return Builder.Uri;
        }

        public static Node CrateInstance(string protocol, string host, int port, string path)
        {
            return new Node(protocol, host, port, path);
        }
    }
}
