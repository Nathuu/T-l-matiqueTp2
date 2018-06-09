using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class LSStrategy : INetworkStrategy
    {
        public void PrepareNetwork(string name, RoutingTable routingTable, Dictionary<string, TcpClient> senders, string sendingRouter, List<Router> routers)
        {
            List<Entry> visited = new List<Entry> { new Entry(name, 0, 0, "") };
            
            Entry currentNode = routingTable.findMinimalDistanceNode(visited);

            while (visited.Count < 6)
            {
                Router currentRouter = routers.Find(router => router.name == currentNode.name);
                visited.Add(currentNode);

                foreach (var entry in currentRouter.routingTable.entries)
                {
                    if (entry.Value.cost < Constant.INFINITY && entry.Key != name)
                    {
                        if (entry.Value.cost + routingTable.entries[currentNode.name].cost < routingTable.entries[entry.Value.name].cost)
                        {
                            int newCost = entry.Value.cost + routingTable.entries[currentNode.name].cost;
                            string newRoute = routingTable.entries[currentNode.name].nextHop;

                            routingTable.entries[entry.Value.name].cost = newCost;
                            routingTable.entries[entry.Value.name].nextHop = newRoute;

                        }
                    }
                }
                currentNode = currentRouter.routingTable.findMinimalDistanceNode(visited);
            }
            Console.WriteLine(routingTable.ToString());
        }
    }
}
