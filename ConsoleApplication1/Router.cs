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
        public string hostName;
        public string name;
        public TcpListener listener;
        TcpClient newClient;
        public Dictionary<string, TcpClient> senders;
        public Boolean networkEstablished;

        public Router(string name, RoutingTable rTable, string hostName, IPEndPoint endPoint)
        {
            this.name = name;
            this.routingTable = rTable;
            this.hostName = hostName;
            this.senders = new Dictionary<string, TcpClient>();
            this.listener = new TcpListener(endPoint);
            this.networkEstablished = false;
            this.listener.Start();

        }

        public Router()
        {
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
                Console.WriteLine(e);
                Connect(endPointName, ipAdress, port);
            }
        }

        public void DiscoverNetwork(string sendindRouter)
        {
            Console.WriteLine(name + " Routing table: ");
            Console.Write(routingTable.ToString());

            foreach (var sender in senders)
            {
                StreamWriter sWriter = new StreamWriter(sender.Value.GetStream(), Encoding.ASCII);
                if (sendindRouter != sender.Key) { 
                    foreach (var entry in routingTable.entries)
                    {
                        if (sender.Key != entry.Key 
                            && entry.Value.cost < Constant.INFINITY
                            && sender.Key != "1" && sender.Key != "2" )
                        {
                            sWriter.WriteLine("DV@" + entry.Key + "@" + entry.Value.cost + "@" + entry.Value.port + "@" + name);
                            sWriter.Flush();
                        }
                    }
                }
            }
        }

        // On recoit un message, on l'analyse, et on l'envoie sur la bonne route 
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
                    string sendingRouter = splittedMessaege[4];

                    networkEstablished = false;
                    if (!routingTable.entries.ContainsKey(entryName))
                    {
                        routingTable.entries.Add(entryName, new Entry(entryName, entryPort, entryCost, ""));
                        DiscoverNetwork(sendingRouter);
                    }
                    else {
                        //compare
                        Console.WriteLine("cost " + entryCost + " oldCost " + routingTable.entries[entryName].cost);
                        if(routingTable.entries[sendingRouter].cost+ entryCost < routingTable.entries[entryName].cost)
                        {
                            Console.WriteLine("change");
                            // cout de router qu'on recoit plus cout quil envoit
                            routingTable.entries[entryName].cost = (routingTable.entries[sendingRouter].cost + entryCost);
                            // router qui envoie
                            routingTable.entries[entryName].nextHop = sendingRouter;
                            routingTable.entries[entryName].port = entryPort;
                            DiscoverNetwork(sendingRouter);
                        }


                    }

                    networkEstablished = true;
                }
                else {
                    string header1 = splittedMessaege[0];
                    string header2 = splittedMessaege[1];
                    string data = splittedMessaege[2];

                    Console.WriteLine(name + " is recieving: " + data + " with final destination: " + header1 + " " + header2);
                    Console.WriteLine(routingTable.ToString());

                    KeyValuePair<string, Entry> entry = routingTable.entries.FirstOrDefault(x => x.Value.port == int.Parse(header1));

                    string nextHop;

                    if (entry.Equals(default(KeyValuePair<string, Entry>)))
                    {
                        nextHop = header2;
                    }
                    else
                    {
                        nextHop = routingTable.entries.FirstOrDefault(x => x.Value.port == int.Parse(header1)).Value.nextHop;
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
