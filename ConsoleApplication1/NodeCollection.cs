using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeCollectionSolution
{
    public class NodeCollection
    {
        public const int INFINITY = 100000;

        public Dictionary<string, Node> nodes = new Dictionary<string, Node>()
        { 
            {"A", new Node("A", INFINITY, "")},
            {"B", new Node("B", INFINITY, "")},
            {"C", new Node("C", INFINITY, "")},
            {"D", new Node("D", INFINITY, "")},
            {"E", new Node("E", INFINITY, "")},
            {"F", new Node("F", INFINITY, "")}
        };

        public Node findMinimalAdjacentNode()
        {
            Node minimum = new Node("default", INFINITY, "");
            foreach(var node in nodes)
            {
                if (node.Value.cost < minimum.cost && node.Value.cost < INFINITY)
                {
                    minimum = node.Value;
                }
            }
            return minimum;
        }
    }
}
