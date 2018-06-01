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
      
        static void Main(string[] args)
        {
            ArrayList routers = new ArrayList();
            ArrayList hosts = new ArrayList();
            Console.WriteLine("Hello World");
            init(routers, hosts);

            Console.WriteLine("Done");
            while (true) { }
        }

        public static void init(ArrayList routers, ArrayList hosts)
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
        }

        public static Router addRouter(Dictionary<string, int> nodes, string name, string hostName)
        {
            NodeCollection routerNodes = new NodeCollection();

            foreach (var node in nodes)
            {
                routerNodes.nodes[node.Key] = node.Value;
            }

            routerNodes.nodes.Remove(name);

            return new Router(name, routerNodes, hostName);
        }


        public static void initLS()
        {
            //1 Initialization:
            //2 N = { A}
            //3 for all nodes v
            //4 if v adjacent to A
            //5 then D(v) = c(A, v)
            //6 else D(v) = ∞
            //7
            //8 Loop
            //9 find w not in N such that D(w) is a minimum
            //10 add w to N
            //11 update D(v) for all v adjacent to w and not in N:
            //12 D(v) = min(D(v), D(w) + c(w, v))
            //13 /* new cost to v is either old cost to v or known
            //14 shortest path cost to w plus cost from w to v */
            //15 until all nodes in N
        }

        public static  void initDV()
        {

        }
    }
}
