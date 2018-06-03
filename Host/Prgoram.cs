using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace HostSolution
{
    public class Program
    {   
        static void Main(string[] args)
        {
            Host host;

            string hostName = Console.ReadLine();
            int port = Int32.Parse(Console.ReadLine());

            host = new Host(hostName, port);

            Console.WriteLine("New host " + host.hostName);
            Console.WriteLine("Attempting to connect to 127.0.0.1:" + host.port + " ...");

            //1) start connection with router
            //2) Send Message to destation (2)

            host.HelloWorld();
            
            while (true)
            {

            }

        }
    }
}
