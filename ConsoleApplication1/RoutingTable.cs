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
        public Dictionary<string, Entry> entries;

        public RoutingTable()
        {
            entries = new Dictionary<string, Entry>();
        }

        public RoutingTable(string routerName)
        {
            entries = new Dictionary<string, Entry>()
            {
                { "40000", new Entry("40000", Constant.INFINITY, "" )},
                {"40100", new Entry("40100", Constant.INFINITY, "" )},
                {"40200", new Entry("40200", Constant.INFINITY, "" )},
                {"40300", new Entry("40300", Constant.INFINITY, "" )},
                {"40400", new Entry("40400", Constant.INFINITY, "" )},
                {"40500", new Entry("40500", Constant.INFINITY, "" )},
                {"40600", new Entry("40600", Constant.INFINITY, "" )},
                {"40700", new Entry("40700", Constant.INFINITY, "" )},
                {"40800", new Entry("40800", Constant.INFINITY, "" )},
                {"40900", new Entry("40900", Constant.INFINITY, "" )},
            };
        }

        

        public Entry findMinimalAdjacentNode()
        {
            Entry minimum = new Entry("default", Constant.INFINITY, "");
            foreach(var entry in entries)
            {
                if (entry.Value.cost < minimum.cost && entry.Value.cost < Constant.INFINITY && entry.Value.cost > 0)
                {
                    minimum = entry.Value;
                }
            }
            return minimum;
        }

        
        public override string ToString()
        {
            string returnValue = "";
            foreach(var entry in entries)
            {
                returnValue += "Entry: " + entry.Value.name + " Cost: " + entry.Value.cost + " NextHop: " + entry.Value.nextHop + "@";
            }
            returnValue = returnValue.Replace("@", System.Environment.NewLine);
            return returnValue;
        }
    }
}
