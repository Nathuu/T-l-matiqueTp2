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
        public enum PORTS { A = 40000, B = 40100, C = 40200, D = 40300, E = 40400, F = 40500 }

        public Dictionary<string, Entry> entries = new Dictionary<string, Entry>()
        { 
            {"A", new Entry("A", (int)PORTS.A, INFINITY, "")},
            {"B", new Entry("B", (int)PORTS.B, INFINITY, "")},
            {"C", new Entry("C", (int)PORTS.C, INFINITY, "")},
            {"D", new Entry("D", (int)PORTS.D, INFINITY, "")},
            {"E", new Entry("E", (int)PORTS.E, INFINITY, "")},
            {"F", new Entry("F", (int)PORTS.F, INFINITY, "")}
        };

        public Entry findMinimalAdjacentNode(List<Entry> visited)
        {
            Entry minimum = new Entry("default", 0, INFINITY, "");
            foreach(var node in entries)
            {
                if (node.Value.cost < minimum.cost && node.Value.cost < INFINITY && visited.Find(x => x.name == node.Key) == null)
                {
                    minimum = node.Value;
                }
            }
            return minimum;
        }
        public Entry findMinimalDistanceNode(List<Entry> visited)
        {
            Entry minimum = new Entry("default", 0, INFINITY+1, "");
            foreach (var node in entries)
            {
                if (node.Value.cost < minimum.cost && visited.Find(x => x.name == node.Key) == null)
                {
                    minimum = node.Value;
                }
            }
            return minimum;
        }

        public override string ToString()
        {
            string returnValue = "";
            foreach (var entry in entries)
            {
                returnValue += "Entry: " + entry.Value.name + " Port: " + entry.Value.port + " Cost: " + entry.Value.cost + " NextHop: " + entry.Value.nextHop + "@";
            }
            returnValue = returnValue.Replace("@", System.Environment.NewLine);
            return returnValue;
        }
    }
}
