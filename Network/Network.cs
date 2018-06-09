using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using HostSolution;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Network
{
    class Program
    {  
        static void Main(string[] args)
        {
            NetworkConfigurator.Init(args[0]);
            while (true) { }
        }

    }
}
