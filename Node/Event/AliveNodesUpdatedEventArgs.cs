using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware.Node.Event
{
    class AliveNodesUpdatedEventArgs : EventArgs
    {
        public List<Uri> aliveNodeUrls { get; set; }
    }
}
