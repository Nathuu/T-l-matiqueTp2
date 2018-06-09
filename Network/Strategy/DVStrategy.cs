using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class DVStrategy : INetworkStrategy
    {
        private static readonly object ConsoleWriterLock = new object();

        public void PrepareNetwork(string name, RoutingTable routingTable, Dictionary<string, TcpClient> senders, string sendingRouter, List<Router> routers)
        {
            var itemsToRemove = routingTable.entries.Where(x => x.Value.cost == Constant.INFINITY).ToArray();
            foreach (var item in itemsToRemove)
            {
                routingTable.entries.Remove(item.Key);
            }

            lock (ConsoleWriterLock)
            {
                Console.WriteLine(name + " Routing table: ");
                Console.Write(routingTable.ToString());
            }

            foreach (var sender in senders)
            {
                StreamWriter sWriter = new StreamWriter(sender.Value.GetStream(), Encoding.ASCII);
                if (sendingRouter != sender.Key)
                {
                    foreach (var entry in routingTable.entries)
                    {
                        if (sender.Key != entry.Key
                            && entry.Value.cost < Constant.INFINITY
                            && sender.Key != "1" && sender.Key != "2")
                        {
                            sWriter.WriteLine("DV@" + entry.Key + "@" + entry.Value.cost + "@" + entry.Value.port + "@" + name);
                            sWriter.Flush();
                        }
                    }
                }
            }
        }
    }
}
