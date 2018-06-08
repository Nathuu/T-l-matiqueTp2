using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using HostSolution;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Network;

namespace Network
{
    public class Program
    {
        public enum PORTS { A=40000, B, C, D, E, F, ONE, TWO }
        public enum SUBNETWORK { ONEA= 40000, AB= 40100, BC=40200, AD = 40300, CF =40400,  DC=40500, BE=40600, DE=40700, EF=40800, FTWO=40900 }
        public enum ROUTINGMODE { LS = 1, DV = 2 }

        static void Main(string[] args)
        {
            List<Router> routers = new List<Router>();
            ArrayList hosts = new ArrayList();
            Console.WriteLine("Hello World");
            Init(routers, hosts);
            while (true) { }
        }

        public static void Init(List<Router> routers, ArrayList hosts)
        {
           
            if (true)//ls or dv
            {
                initRouters(routers, (int)ROUTINGMODE.LS);
                InitLS(routers);

                Router startRouter = new Router();

                startRouter = routers.Find(x => x.name == "A");

                Console.WriteLine("Done");
                //Console.WriteLine(startRouter.routingTable.entries["F"].nextHop);
            }
        }

        public static void initRouters(List<Router> routers, int routingMode)
        {
            Router router = CreateRouter(new List<Entry> {
                {new Entry(SUBNETWORK.ONEA, 0, "")},
                {new Entry(SUBNETWORK.AB, 0, "")},
                {new Entry(SUBNETWORK.AD, 0, "")},
                {new Entry(SUBNETWORK.BC, 5, "B")},
                {new Entry(SUBNETWORK.BE, 5, "B")},
                {new Entry(SUBNETWORK.DC, 45, "D")},
                {new Entry(SUBNETWORK.DE, 45, "D")}
            }, "A", "1", routingMode);

            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.ONEA + 2));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.AB + 1));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.AD + 1));
            routers.Add(router);

            router = CreateRouter(new List<Entry> {
                {new Entry(SUBNETWORK.AB, 0, "")},
                {new Entry(SUBNETWORK.BC, 0, "")},
                {new Entry(SUBNETWORK.BE, 0, "")},
                {new Entry(SUBNETWORK.ONEA, 5, "A")},
                {new Entry(SUBNETWORK.AD, 5, "A")},
                { new Entry(SUBNETWORK.CF, 70, "C")},
                { new Entry(SUBNETWORK.DC, 70, "C")},
                {new Entry(SUBNETWORK.EF, 3, "E")},
                {new Entry(SUBNETWORK.DE, 3, "E")},
            }, "B", null, routingMode);
            
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.AB + 2));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.BC + 1));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.BE + 1));
            routers.Add(router);

            router = CreateRouter(new List<Entry> {
                {new Entry(SUBNETWORK.BC, 0, "")},
                {new Entry(SUBNETWORK.DC, 0, "")},
                {new Entry(SUBNETWORK.CF, 0, "")},
                {new Entry(SUBNETWORK.AB, 70, "B")},
                {new Entry(SUBNETWORK.BE, 70, "B")},
                {new Entry(SUBNETWORK.AD, 50, "D")},
                {new Entry(SUBNETWORK.DE, 50, "D")},
                {new Entry(SUBNETWORK.EF, 78, "F")},
                {new Entry(SUBNETWORK.FTWO, 78, "F")},
            }, "C", null, routingMode);
            
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.BC + 2));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.DC + 2));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.CF + 1));
            routers.Add(router);

            router = CreateRouter(new List<Entry> {
                {new Entry(SUBNETWORK.AD, 0, "")},
                {new Entry(SUBNETWORK.DC, 0, "")},
                {new Entry(SUBNETWORK.DE, 0, "")},
                {new Entry(SUBNETWORK.AB, 45, "A")},
                {new Entry(SUBNETWORK.ONEA, 45, "A")},
                {new Entry(SUBNETWORK.BC, 50, "C")},
                {new Entry(SUBNETWORK.CF, 50, "C")},
                {new Entry(SUBNETWORK.BE, 8, "E")},
                {new Entry(SUBNETWORK.EF, 8, "E")}
            }, "D", null, routingMode);

            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.AD + 2));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.DC + 1));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.DE + 1));
            routers.Add(router);

            router = CreateRouter(new List<Entry> {
                {new Entry(SUBNETWORK.BE, 0, "")},
                {new Entry(SUBNETWORK.DE, 0, "")},
                {new Entry(SUBNETWORK.EF, 0, "")},
                {new Entry(SUBNETWORK.AB, 3, "B")},
                {new Entry(SUBNETWORK.BC, 3, "B")},
                {new Entry(SUBNETWORK.AD, 8, "D")},
                {new Entry(SUBNETWORK.DC, 8, "D")},
                {new Entry(SUBNETWORK.CF, 7, "F")},
                {new Entry(SUBNETWORK.FTWO, 7, "F")},
            }, "E", null, routingMode);

            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.DE + 2));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.BE + 2));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.EF + 1));
            routers.Add(router);

            router = CreateRouter(new List<Entry> {
                {new Entry(SUBNETWORK.CF, 0, "")},
                {new Entry(SUBNETWORK.EF, 0, "")},
                {new Entry(SUBNETWORK.FTWO, 0, "")},
                {new Entry(SUBNETWORK.DC, 78, "C")},
                {new Entry(SUBNETWORK.BC, 78, "C")},
                {new Entry(SUBNETWORK.BE, 7, "E")},
                {new Entry(SUBNETWORK.DE, 7, "E")},
            }, "F", "2", routingMode);
            
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.CF + 2));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.EF + 2));
            router.initListener(new IPEndPoint(IPAddress.Loopback, (int)SUBNETWORK.FTWO + 1));
            routers.Add(router);

            RouterStartListening(routers);
            ConnectRouters(routers);
        }

        private static void RouterStartListening(List<Router> routers)
        {
            foreach(var router in routers)
            {
                foreach(var listener in router.listeners)
                {
                    Thread th = new Thread(() => router.Listen(listener.Key));
                    th.Start();
                }
                
                
            }
        }

        public static Router CreateRouter(List<Entry> neighbours, string name, string hostName, int routingMode)
        {
            RoutingTable routingTable;

            if (routingMode == (int)ROUTINGMODE.DV)
            {
                routingTable = new RoutingTable();
            }
            else
            {
                routingTable = new RoutingTable(name);
            }
            

            foreach (var neighbour in neighbours)
            {
                routingTable.entries[neighbour.name] = neighbour;
            }

            return new Router(name, routingTable, hostName);
        }

        public static void ConnectRouters(List<Router> routers)
        {
            routers.Find(x => x.name == "A").Connect("1", "127.0.0.1", (int)SUBNETWORK.ONEA + 1);
            routers.Find(x => x.name == "A").Connect("B", "127.0.0.1", (int)SUBNETWORK.AB + 2);
            routers.Find(x => x.name == "A").Connect("D", "127.0.0.1", (int)SUBNETWORK.AD + 2);

            routers.Find(x => x.name == "B").Connect("A", "127.0.0.1", (int)SUBNETWORK.AB + 1);
            routers.Find(x => x.name == "B").Connect("C", "127.0.0.1", (int)SUBNETWORK.BC + 2);
            routers.Find(x => x.name == "B").Connect("E", "127.0.0.1", (int)SUBNETWORK.BE + 2);

            routers.Find(x => x.name == "C").Connect("B", "127.0.0.1", (int)SUBNETWORK.BC + 1);
            routers.Find(x => x.name == "C").Connect("D", "127.0.0.1", (int)SUBNETWORK.DC + 1);
            routers.Find(x => x.name == "C").Connect("F", "127.0.0.1", (int)SUBNETWORK.CF + 2);

            routers.Find(x => x.name == "D").Connect("A", "127.0.0.1", (int)SUBNETWORK.AD + 1);
            routers.Find(x => x.name == "D").Connect("C", "127.0.0.1", (int)SUBNETWORK.DC + 2);
            routers.Find(x => x.name == "D").Connect("E", "127.0.0.1", (int)SUBNETWORK.DE + 2);

            routers.Find(x => x.name == "E").Connect("B", "127.0.0.1", (int)SUBNETWORK.BE + 1);
            routers.Find(x => x.name == "E").Connect("D", "127.0.0.1", (int)SUBNETWORK.DE + 1);
            routers.Find(x => x.name == "E").Connect("F", "127.0.0.1", (int)SUBNETWORK.EF + 2);

            routers.Find(x => x.name == "F").Connect("C", "127.0.0.1", (int)SUBNETWORK.CF + 1);
            routers.Find(x => x.name == "F").Connect("E", "127.0.0.1", (int)SUBNETWORK.EF + 1);
            routers.Find(x => x.name == "F").Connect("2", "127.0.0.1", (int)SUBNETWORK.FTWO + 2);
        }
        
        public static void InitLS(List<Router> routers)
        {

            //foreach(var router in routers)
            //{
            //    RunLsAlgorithm(router.name, routers);
            //}
            RunLsAlgorithm("A", routers);

        }

        private static void RunLsAlgorithm(string routerName, List<Router> routers)
        {
            List<Entry> visited = new List<Entry>();            
            Router startRouter = new Router();

            startRouter = routers.Find(x => x.name == routerName);

            foreach (var entry in startRouter.routingTable.entries)
            {
                if (entry.Value.cost == 0)
                {
                    visited.Add(entry.Value);
                }
            }
            

            Entry currentNode = startRouter.routingTable.findMinimalAdjacentNode();

            Console.WriteLine("Router " + startRouter.name + " state before LS");
            Console.WriteLine(startRouter.routingTable.ToString());
                            
            while (visited.Count < 11)
            {
                Router currentRouter = new Router();

                visited.Add(currentNode);

                currentRouter = routers.Find(router => router.name == currentNode.nextHop);

                foreach (var entry in currentRouter.routingTable.entries)
                {
                    if (entry.Value.cost < Constant.INFINITY
                        && !visited.Any(x => x.name == entry.Value.name)
                        && entry.Value.cost > 0)
                    {

                        if (entry.Value.cost + startRouter.routingTable.entries[currentNode.name].cost < startRouter.routingTable.entries[entry.Value.name].cost)
                        {
                            int newCost = entry.Value.cost + startRouter.routingTable.entries[currentNode.name].cost;

                            startRouter.routingTable.entries[entry.Value.name].cost = newCost;
                            startRouter.routingTable.entries[entry.Value.name].nextHop = currentNode.nextHop;
                        }
                    }
                }
                currentNode = currentRouter.routingTable.findMinimalAdjacentNode();
            }
            Console.WriteLine("Router " + startRouter.name + " state after LS");
            Console.WriteLine(startRouter.routingTable.ToString());
        }

        public static  void InitDV()
        {

        }
    }
}
