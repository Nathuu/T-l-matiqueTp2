using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodeCollectionSolution; 

namespace RouterSolution
{
    public class Router
    {
        private NodeCollection nodes;        
        private string hostName;
        private string name;

        public Router(string name, NodeCollection nodes, string hostName)
        {
            this.name = name;
            this.nodes = nodes;            
            this.hostName = hostName;
        }

        public void negotiateNetwork()
        {

        }
        void Main(string[] args)
        {
        }
    }
}
