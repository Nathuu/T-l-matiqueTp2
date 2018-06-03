using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Network
{
    public class Router
    {
        public RoutingTable rTable;
        public string hostName;
        public string name;
        public TcpClient receiver;
        public Dictionary<string, TcpClient> senders;

        public Router(string name, RoutingTable rTable, string hostName, IPEndPoint endPoint)
        {
            this.name = name;
            this.rTable = rTable;
            this.hostName = hostName;
            this.receiver = new TcpClient(endPoint);
            this.senders = new Dictionary<string, TcpClient>();
        }

        public Router()
        {
        }

        public void NegotiateNetwork()
        {

        }

        public void Connect(string endPointName, string ipAdress, int port)
        {
            senders.Add(endPointName, new TcpClient(ipAdress, port));
        }

        public void SendMessage()
        {

        }

        void Main(string[] args)
        {
        }
    }
}
