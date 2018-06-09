using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace Network
{
    public class Router
    {
        public RoutingTable routingTable;
        public INetworkStrategy networkStrategie;
        public List<Router> routers;
        public string hostName;
        public string name;
        public string sendingRouter;

        public TcpListener listener;
        public TcpClient newClient;

        public Dictionary<string, TcpClient> senders;
        public Boolean networkEstablished;
        
        public Router(string name, RoutingTable rTable, string hostName, IPEndPoint endPoint, INetworkStrategy strategy)
        {
            this.name = name;
            this.sendingRouter = name;
            this.routingTable = rTable;
            this.hostName = hostName;
            this.senders = new Dictionary<string, TcpClient>();
            this.listener = new TcpListener(endPoint);
            this.networkEstablished = false;
            this.networkStrategie = strategy;
            this.listener.Start();
        }

        public void Listen()
        {
            Console.WriteLine("Router: " + name + " now listening");
            while (true) { 
                newClient = listener.AcceptTcpClient();
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public void Connect(string endPointName, string ipAdress, int port)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", port);
                senders.Add(endPointName, client);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                Connect(endPointName, ipAdress, port);
            }
        }

        public void PrepareNetwork(List<Router> routers)
        {
            this.routers = routers;
            networkStrategie.PrepareNetwork(name, routingTable, senders, sendingRouter, routers);
        }
        
        public void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);

            Boolean bClientConnected = true;
            String message = null;

            while (bClientConnected)
            {
                message = sReader.ReadLine();
                string[] splittedMessaege = message.Split('@');
                               
                if (splittedMessaege[0] == "DV")
                {
                    string entryName = splittedMessaege[1];
                    int entryCost = int.Parse(splittedMessaege[2]);
                    int entryPort = int.Parse(splittedMessaege[3]);
                    this.sendingRouter = splittedMessaege[4];

                    networkEstablished = false;
                    if (!routingTable.entries.ContainsKey(entryName))
                    {
                        routingTable.entries.Add(entryName, new Entry(entryName, entryPort, entryCost, sendingRouter));
                        PrepareNetwork(routers);
                    }
                    else {
                        if(routingTable.entries[sendingRouter].cost + entryCost < routingTable.entries[entryName].cost)
                        {                               
                            routingTable.entries[entryName].cost = (routingTable.entries[sendingRouter].cost + entryCost);
                            routingTable.entries[entryName].nextHop = sendingRouter;
                            routingTable.entries[entryName].port = entryPort;
                            PrepareNetwork(routers);
                        }
                    }

                    networkEstablished = true;
                }
                else {
                    string destinationNetwork = splittedMessaege[0];
                    string destinationHost = splittedMessaege[1];
                    string data = splittedMessaege[2];
                    string fullAddress = destinationNetwork.Remove(destinationNetwork.Length - 1, 1) + destinationHost;

                    Console.WriteLine(name + " is recieving: " + data + " with final destination: " + fullAddress);
                    Console.WriteLine(routingTable.ToString());

                    KeyValuePair<string, Entry> entry = routingTable.entries.FirstOrDefault(x => x.Value.port == int.Parse(destinationNetwork));

                    string nextHop;

                    if (entry.Equals(default(KeyValuePair<string, Entry>)))
                    {
                        nextHop = destinationHost;
                    }
                    else
                    {
                        nextHop = routingTable.entries.FirstOrDefault(x => x.Value.port == int.Parse(destinationNetwork)).Value.nextHop;
                    }             

                    TcpClient nextClient = senders[nextHop];
                    StreamWriter sWriter = new StreamWriter(nextClient.GetStream(), Encoding.ASCII);
                
                    sWriter.WriteLine(message);
                    sWriter.Flush();
                }
            }
        }

    }
}
