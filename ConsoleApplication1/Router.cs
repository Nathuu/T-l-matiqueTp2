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
        public Dictionary<int, TcpListener> listeners;
        public Dictionary<string, TcpClient> senders;
        public int routingMode;
        public Boolean networkEstablished;

        private StreamReader SReader;
        private StreamWriter SWriter;

        public Router(string name, RoutingTable rTable, string hostName)
        {
            this.name = name;
            this.routingTable = rTable;
            this.hostName = hostName;
            this.senders = new Dictionary<string, TcpClient>();
            this.listeners = new Dictionary<int, TcpListener>();
        }

        public Router()
        {
        }

        public void initListener(IPEndPoint endPoint)
        {
            TcpListener listener = new TcpListener(endPoint);
            listener.Start();
            this.listeners[endPoint.Port] = listener;
        }
        
        public void Listen(int listenerKey)
        {
            Console.WriteLine("Router: " + name + " now listening on " + listenerKey);
            while (true) { 
                TcpClient newClient = listeners[listenerKey].AcceptTcpClient();
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
            }catch(Exception e)
            {
                Console.WriteLine(e);
                while (true)
                {

                }
            }
            
        }

        // ref
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
                string header1 = splittedMessaege[0];
                string header2 = splittedMessaege[1];
                string data = splittedMessaege[2];

                if(header1 == "LSA")
                {
                    networkEstablished = false;
                    if (!routingTable.entries.ContainsKey(data))
                    {
                        routingTable.entries.Add(data, new Entry(data, Constant.INFINITY, ""));
                        DiscoverNetwork();
                    }
                    
                    networkEstablished = true;
                }
                else
                {
                    Console.WriteLine(name + " is recieving: " + data + " with final destination: " + header1 + " " + header2);
                    Console.WriteLine(routingTable.ToString());

                    string nextHop = routingTable.entries[header1].nextHop;

                    // Local network adress, route it accordingly
                    if (nextHop == "")
                    {
                        nextHop = (int.Parse(header1) + int.Parse(header2)).ToString();
                    }

                    TcpClient nextClient = senders[nextHop];
                    StreamWriter sWriter = new StreamWriter(nextClient.GetStream(), Encoding.ASCII);


                    sWriter.Write(message);
                }

            }
        }
        
        public void DiscoverNetwork()
        {
            Console.WriteLine(name + " Routing table: ");
            Console.Write(routingTable.ToString());

            foreach(var sender in senders)
            {
                StreamWriter sWriter = new StreamWriter(sender.Value.GetStream(), Encoding.ASCII);

                foreach (var entry in routingTable.entries)
                {
                    if (sender.Key != entry.Key)
                    {
                        sWriter.WriteLine("DV@" + entry.Key + "@" + entry.Value.cost);
                        sWriter.Flush();
                    }
                }
            }
        }

    }
}
