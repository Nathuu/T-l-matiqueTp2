using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace HostSolution
{
    public class Host
    {
        public string hostName;
        public int port;
        TcpClient client;
        private StreamWriter SWriter;

        public Host()
        {

        }

        public Host(string hostName, int port)
        {
            this.hostName = hostName;
            this.port = port;
            client = new TcpClient("127.0.0.1", port);
        }

        public void HelloWorld()
        {   
            SWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);

            SWriter.WriteLine("Hello World");
            SWriter.Flush();
        }
    }
}
