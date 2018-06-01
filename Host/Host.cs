using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostSolution
{
    public class Host
    {
        private string hostName;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");

        }

        public Host(string hostName)
        {
            this.hostName = hostName;
        }
    }
}
