using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network
{
    public static class NetworkConfigurator
    {
        public enum PORTS { A = 40000, B = 40100, C = 40200, D = 40300, E = 40400, F = 40500, ONE = 40001, TWO = 40501 }

        public static void Init(string strategy)
        {
            List<Router> routers = new List<Router>();
            CreateRouters(routers, strategy);
            StartListeningRouters(routers);
            ConnectRouters(routers);

            foreach (var router in routers)
            {
                router.PrepareNetwork(routers);
            }
        }

        private static void CreateRouters(List<Router> routers, string strategy)
        {
            routers.Add(AddRouter(new Dictionary<string, int> {
                {"B", 5 },
                {"D", 45 }
            }, "A", "1",
            new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.A),
            strategy));

            routers.Add(AddRouter(new Dictionary<string, int> {
                {"A", 5 },
                {"C", 70 },
                {"E", 3 }
            }, "B", null, new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.B),
            strategy));

            routers.Add(AddRouter(new Dictionary<string, int> {
                {"B", 70 },
                {"D", 50 },
                {"F", 78 }
            }, "C", null, new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.C),
            strategy));

            routers.Add(AddRouter(new Dictionary<string, int> {
                {"A", 45},
                {"C", 50 },
                {"E", 8 }
            }, "D", null, new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.D),
            strategy));

            routers.Add(AddRouter(new Dictionary<string, int> {
                {"B", 3 },
                {"D", 8},
                {"F", 7 }
            }, "E", null, new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.E),
            strategy));

            routers.Add(AddRouter(new Dictionary<string, int> {
                {"C", 78 },
                {"E", 7}
            }, "F", "2", new IPEndPoint(IPAddress.Parse("127.0.0.1"), (int)PORTS.F),
            strategy));
        }

        private static void StartListeningRouters(List<Router> routers)
        {
            foreach (var router in routers)
            {
                Thread th = new Thread(router.Listen);
                th.Start();
            }
        }

        public static Router AddRouter(Dictionary<string, int> neighbours, string name, string hostName, IPEndPoint ipEndPoint, string strategy)
        {
            RoutingTable routerNodes = new RoutingTable();

            foreach (var neighbour in neighbours)
            {
                routerNodes.entries[neighbour.Key].cost = neighbour.Value;
                routerNodes.entries[neighbour.Key].nextHop = neighbour.Key;
            }

            routerNodes.entries.Remove(name);

            if (strategy == "LS")
            {
                return new Router(name, routerNodes, hostName, ipEndPoint, new LSStrategy());
            }
            else
            {
                return new Router(name, routerNodes, hostName, ipEndPoint, new DVStrategy());
            }
        }

        public static void ConnectRouters(List<Router> routers)
        {
            routers.Find(x => x.name == "A").Connect("1", "127.0.0.1", (int)PORTS.ONE);
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
            routers.Find(x => x.name == "F").Connect("2", "127.0.0.1", (int)PORTS.TWO);
        }
    }
}
