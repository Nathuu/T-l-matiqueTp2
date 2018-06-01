using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodeCollectionSolution; 

namespace Network
{
    public class Router
    {
        public NodeCollection nodes;
        public string hostName;
        public string name;

        public Router(string name, NodeCollection nodes, string hostName)
        {
            this.name = name;
            this.nodes = nodes;            
            this.hostName = hostName;
        }

        public Router()
        {
        }

        public void negotiateNetwork()
        {

        }
        void Main(string[] args)
        {
        }
    }
}
