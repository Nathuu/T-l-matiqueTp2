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

            Console.Write("Enter your hostname: ");
            string hostName = args[0];
            int port = 0;
            string destination = "";
            if (hostName == "1")
            {
                port = 40001;
                destination = "2";
            }
            else if (hostName == "2")
            {
                port = 40501;
            }
            host = new Host(hostName, port, destination);

            Console.WriteLine("New host " + host.hostName);
            Console.WriteLine("Attempting to connect to 127.0.0.1:" + host.port + " ...");

            host.ConnectToRouter();
            
            if (hostName == "1")
            {
                host.HelloWorld();
            }

            while (true)
            {

            }

        }
    }
}
