using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace HostSolution
{
    public class Host
    {
        public string hostName;
        public int port;
        TcpClient client;
        private string destination;
        private StreamWriter SWriter;
        private TcpListener listener;

        public Host()
        {

        }

        public Host(string hostName, int port, string destination)
        {
            this.hostName = hostName;
            this.port = port;
            this.destination = destination;
            listener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            listener.Start();
            Thread th = new Thread(Listen);
            th.Start();
        }

        public void Listen()
        {
            Console.WriteLine("Host is now on port " + port);
            while (true)
            {
                TcpClient newClient = listener.AcceptTcpClient();
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);

            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);

            Boolean bClientConnected = true;
            String message = null;

            while (bClientConnected)
            {
                message = sReader.ReadLine();
                string[] splittedMessaege = message.Split('@');
                string header1 = splittedMessaege[0];
                string header2 = splittedMessaege[1];
                string data = splittedMessaege[2];

                Console.WriteLine(hostName + " is recieving: " + data);

            }
        
        }

        public void HelloWorld()
        {
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("Now Sending message...");
            SWriter.WriteLine("40500@2@Hello World");
            SWriter.Flush();
        }

        public void ConnectToRouter()
        {
            try
            {
                client = new TcpClient("127.0.0.1", port -1);
                SWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
                Console.WriteLine("Connected!");
            }
            catch (Exception e)
            {
                ConnectToRouter();
            }
        }
    }
}
