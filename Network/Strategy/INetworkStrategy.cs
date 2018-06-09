using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public interface INetworkStrategy
    {
        void PrepareNetwork(string name, RoutingTable routingTable, Dictionary<string, TcpClient> senders, string sendingRouter, List<Router> routers);

    }
}
