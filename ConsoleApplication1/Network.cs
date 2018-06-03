using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using HostSolution;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Network
{
    class Program
    {
        public const int INFINITY = 100000; 
        public enum PORTS { A=40000, B, C, D, E, F }

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
            
            routers.Add(AddRouter(new Dictionary<string, int> {
                {"B", 5 },
                {"D", 45 }
            }, "A", "1", new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.A)));

            routers.Add(AddRouter(new Dictionary<string, int> {
                {"A", 5 },
                {"C", 70 },
                {"E", 3 }
            }, "B", null, new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.B)));

            routers.Add(AddRouter(new Dictionary<string, int> {
                {"B", 70 },
                {"D", 50 },
                {"F", 78 }
            }, "C", null, new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.C)));

            routers.Add(AddRouter(new Dictionary<string, int> {
                {"A", 45},
                {"C", 50 },
                {"E", 8 }
            }, "D", null, new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.D)));

            routers.Add(AddRouter(new Dictionary<string, int> {                
                {"B", 3 },
                {"D", 8},
                {"F", 7 }
            }, "E", null, new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.E)));

            routers.Add(AddRouter(new Dictionary<string, int> {
                {"C", 78 },
                {"E", 7}                
            }, "F", "2", new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.F)));

            RouterStartListening(routers);
            ConnectRouters(routers);
            
            if (true)//ls or dv
            {
                InitLS(routers);

                Router startRouter = new Router();

                startRouter = routers.Find(x => x.name == "A");

                Console.WriteLine("Done");
                Console.WriteLine(startRouter.rTable.entries["F"].route);
            }
        }

        private static void RouterStartListening(List<Router> routers)
        {
            foreach(var router in routers)
            {
                Thread th = new Thread(router.Listen);
                th.Start();
            }
        }

        public static Router AddRouter(Dictionary<string, int> neighbours, string name, string hostName, IPEndPoint ipEndPoint)
        {
            RoutingTable routerNodes = new RoutingTable();

            foreach (var neighbour in neighbours)
            {
                routerNodes.entries[neighbour.Key].cost = neighbour.Value;
                routerNodes.entries[neighbour.Key].route = neighbour.Key;
            }

            routerNodes.entries.Remove(name);

            return new Router(name, routerNodes, hostName, ipEndPoint);
        }

        public static void ConnectRouters(List<Router> routers)
        {
            routers.Find(x => x.name == "A").Connect("B", "127.0.0.1", (int)PORTS.B);
            routers.Find(x => x.name == "A").Connect("D", "127.0.0.1", (int)PORTS.D);

            routers.Find(x => x.name == "B").Connect("A", "127.0.0.1", (int)PORTS.A);
            routers.Find(x => x.name == "B").Connect("C", "127.0.0.1", (int)PORTS.C);
            routers.Find(x => x.name == "B").Connect("E", "127.0.0.1", (int)PORTS.E);

            routers.Find(x => x.name == "C").Connect("B", "127.0.0.1", (int)PORTS.B);
            routers.Find(x => x.name == "C").Connect("D", "127.0.0.1", (int)PORTS.D);
            routers.Find(x => x.name == "C").Connect("F", "127.0.0.1", (int)PORTS.F);

            routers.Find(x => x.name == "D").Connect("A", "127.0.0.1", (int)PORTS.A);
            routers.Find(x => x.name == "D").Connect("C", "127.0.0.1", (int)PORTS.C);
            routers.Find(x => x.name == "D").Connect("E", "127.0.0.1", (int)PORTS.E);

            routers.Find(x => x.name == "E").Connect("B", "127.0.0.1", (int)PORTS.B);
            routers.Find(x => x.name == "E").Connect("D", "127.0.0.1", (int)PORTS.D);
            routers.Find(x => x.name == "E").Connect("F", "127.0.0.1", (int)PORTS.F);

            routers.Find(x => x.name == "F").Connect("C", "127.0.0.1", (int)PORTS.C);
            routers.Find(x => x.name == "F").Connect("E", "127.0.0.1", (int)PORTS.E);
        }
        
        public static void InitLS(List<Router> routers)
        {
            List<Entry> visited = new List<Entry> { new Entry("A", 0, "") };

            Router startRouter = new Router();

            startRouter = routers.Find(x => x.name == "A");

            Entry currentNode = startRouter.rTable.findMinimalAdjacentNode();

            while (visited.Count < 7)
            {                
                Router currentRouter = new Router();

                visited.Add(currentNode);

                currentRouter = routers.Find(router => router.name == currentNode.name);
                
                foreach(var entry in currentRouter.rTable.entries)
                {
                    if(entry.Value.cost < INFINITY && !visited.Any(x => x.name == entry.Value.name))
                    {
                        if(entry.Value.cost + startRouter.rTable.entries[currentNode.name].cost < startRouter.rTable.entries[entry.Value.name].cost)
                        {
                            int newCost = entry.Value.cost + startRouter.rTable.entries[currentNode.name].cost;
                            string newRoute = startRouter.rTable.entries[currentNode.name].route + entry.Value.name;

                            startRouter.rTable.entries[entry.Value.name].cost = newCost;
                            startRouter.rTable.entries[entry.Value.name].route = newRoute;

                        }   
                    }
                }
                currentNode = currentRouter.rTable.findMinimalAdjacentNode();
            }            
      
        }

        public static  void InitDV()
        {

        }
    }
}
