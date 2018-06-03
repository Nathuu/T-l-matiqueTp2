using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace Network
{
    public class Router
    {
        public RoutingTable rTable;
        public string hostName;
        public string name;
        public TcpListener listener;
        public Dictionary<string, TcpClient> senders;

        private StreamReader SReader;
        private StreamWriter SWriter;
        private Boolean isConnected; 

        public Router(string name, RoutingTable rTable, string hostName, IPEndPoint endPoint)
        {
            this.name = name;
            this.rTable = rTable;
            this.hostName = hostName;
            this.senders = new Dictionary<string, TcpClient>();
            this.listener = new TcpListener(endPoint);
            this.listener.Start();

        }

        public Router()
        {
        }

        public void NegotiateNetwork()
        {

        }

        public void Listen()
        {
            Console.WriteLine("Router: " + name + " now listening");
            while (true) { 
                TcpClient newClient = listener.AcceptTcpClient();
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public void Connect(string endPointName, string ipAdress, int port)
        {
            TcpClient client = new TcpClient("127.0.0.1", port);            
            senders.Add(endPointName, client);

            //HandleCommunication(client);
        }

        /** ref **/ 
        //public void HandleCommunication(TcpClient client)
        //{
        //    foreach(var sender in senders)
        //    {
        //        if(name == "A") {
        //            Console.WriteLine("Sending to: " + sender.Key);
        //            SReader = new StreamReader(sender.Value.GetStream(), Encoding.ASCII);
        //            SWriter = new StreamWriter(sender.Value.GetStream(), Encoding.ASCII);

        //            isConnected = true;
        //            String sData = "hello";
           
        //            //sData = Console.ReadLine();

        //            Console.WriteLine(name + " is sending: " + sData);

        //            // write data and make sure to flush, or the buffer will continue to 
        //            // grow, and your data might not be sent when you want it, and will
        //            // only be sent once the buffer is filled.
        //            SWriter.WriteLine(sData);
        //            SWriter.Flush();

        //            // if you want to receive anything
        //            //String sDataIncomming = SReader.ReadLine();
        //        }
        //    }
        //}

        // ref
        // On recoit un message, on l'analyse, et on l'envoie sur la bonne route 
        public void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;

            // sets two streams
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested

            Boolean bClientConnected = true;
            String sData = null;

            while (bClientConnected)
            {   
                sData = sReader.ReadLine();             
                Console.WriteLine(name + " is recieving: " + sData);
                // writer envoie au bon router.
                // comment ecrire un header
                // https://stackoverflow.com/questions/19523088/create-http-request-using-tcpclient/
            }
        }

        public void SendMessage()
        {

        }

        internal void Recieve()
        {
            Console.WriteLine(name);
            Thread.Sleep(10000);
        }
    }
}
