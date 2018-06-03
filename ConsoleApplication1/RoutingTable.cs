using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class RoutingTable
    {
        public const int INFINITY = 100000;

        public Dictionary<string, Entry> entries = new Dictionary<string, Entry>()
        { 
            {"A", new Entry("A", INFINITY, "")},
            {"B", new Entry("B", INFINITY, "")},
            {"C", new Entry("C", INFINITY, "")},
            {"D", new Entry("D", INFINITY, "")},
            {"E", new Entry("E", INFINITY, "")},
            {"F", new Entry("F", INFINITY, "")}
        };

        public Entry findMinimalAdjacentNode()
        {
            Entry minimum = new Entry("default", INFINITY, "");
            foreach(var node in entries)
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
