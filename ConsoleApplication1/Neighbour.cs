using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reseau
{
    public class Node
    {
        public string name;
        public int cost;

        public Node(string name, int cost)
        {
            this.name = name;
            this.cost = cost;
        }
    }
}
