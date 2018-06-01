using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using NodeCollectionSolution;
using HostSolution;

namespace Network
{
    class Program
    {
        public const int INFINITY = 100000; 
        static void Main(string[] args)
        {
            List<Router> routers = new List<Router>();
            ArrayList hosts = new ArrayList();
            Console.WriteLine("Hello World");
            init(routers, hosts);
            while (true) { }
        }

        public static void init(List<Router> routers, ArrayList hosts)
        {
            
            routers.Add(addRouter(new Dictionary<string, int> {
                {"B", 5 },
                {"D", 45 }
            }, "A", "1"));

            routers.Add(addRouter(new Dictionary<string, int> {
                {"A", 5 },
                {"C", 70 },
                {"E", 3 }
            }, "B", null));

            routers.Add(addRouter(new Dictionary<string, int> {
                {"B", 70 },
                {"D", 50 },
                {"F", 78 }
            }, "C", null));

            routers.Add(addRouter(new Dictionary<string, int> {
                {"A", 45},
                {"C", 50 },
                {"E", 8 }
            }, "D", null));

            routers.Add(addRouter(new Dictionary<string, int> {                
                {"B", 3 },
                {"D", 8},
                {"F", 7 }
            }, "E", null));

            routers.Add(addRouter(new Dictionary<string, int> {
                {"C", 78 },
                {"E", 7}                
            }, "F", "2"));
            
            hosts = new ArrayList();
            hosts.Add(new Host("1"));
            hosts.Add(new Host("2"));

            if (true)
            {
                initLS(routers);

                Router startRouter = new Router();

                foreach (var router in routers)
                {
                    if (router.name == "A")
                    {
                        startRouter = router;
                    }
                }

                Console.WriteLine("Done");
                Console.WriteLine(startRouter.nodes.nodes["F"].route);
            }
        }

        public static Router addRouter(Dictionary<string, int> nodes, string name, string hostName)
        {
            NodeCollection routerNodes = new NodeCollection();

            foreach (var node in nodes)
            {
                routerNodes.nodes[node.Key].cost = node.Value;
                routerNodes.nodes[node.Key].route = node.Key;
            }

            routerNodes.nodes.Remove(name);

            return new Router(name, routerNodes, hostName);
        }


        public static void initLS(List<Router> routers)
        {
            List<Node> visited = new List<Node>();          
            visited.Add(new Node("A", 0, ""));

            Router startRouter = new Router(); 

            foreach (var router in routers)
            {
                if (router.name == "A")
                {
                    startRouter = router;
                }
            }

            Node currentNode = startRouter.nodes.findMinimalAdjacentNode();

            while (visited.Count < 7)
            {
                
                Router currentRouter = new Router();

                visited.Add(currentNode);

                foreach (var router in routers)
                {
                    if (router.name == currentNode.name)
                    {
                        currentRouter = router;
                    }
                }

                foreach(var node in currentRouter.nodes.nodes)
                {
                    if(node.Value.cost < INFINITY && !visited.Any(x => x.name == node.Value.name))
                    {
                        if(node.Value.cost + startRouter.nodes.nodes[currentNode.name].cost < startRouter.nodes.nodes[node.Value.name].cost)
                        {
                            startRouter.nodes.nodes[node.Value.name].cost = node.Value.cost + startRouter.nodes.nodes[currentNode.name].cost;
                            startRouter.nodes.nodes[node.Value.name].route = startRouter.nodes.nodes[currentNode.name].route + node.Value.name;

                        }   
                    }
                }
                currentNode = currentRouter.nodes.findMinimalAdjacentNode();
            }            
      
        }

        public static  void initDV()
        {

        }
    }
}
