using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace SimpleTCP
{
    class Program
    {


        static void ProcessServer(int port)
        {
            TcpListener listener = null;
            try
            {
                IPAddress ip = new IPAddress(new byte[] { 0, 0, 0, 0 });
                listener = new TcpListener(ip, port);
                listener.Start();
                var client = listener.AcceptTcpClient();
                Console.WriteLine("Connected from {0}", client.Client.RemoteEndPoint);

                ProcessClient(client);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

        static void ProcessClient(TcpClient client)
        {
            TcpClientReceiver receiver = new TcpClientReceiver(client);
            Thread receiverThread = new Thread(new ThreadStart(receiver.Run));
            receiverThread.Start();
            new TcpClientHandler().Run(client);
            receiverThread.Join();
        }

        static int ReadPort()
        {
            Console.Write("port: ");
            string portStr = Console.ReadLine();
            int port;
            if (!Int32.TryParse(portStr, out port) || port < 1 || port > 65535)
            {
                Console.WriteLine("Incorrect port argument. Number [1;65535] is required");
                Environment.Exit(1);
            }
            return port;
        }

        static IPAddress ReadIP()
        {
            Console.Write("ipAddr: ");
            string portStr = Console.ReadLine();
            IPAddress ipAddress;
            if (!IPAddress.TryParse(portStr, out ipAddress))
            {
                Console.WriteLine("Incorrect ipAddress argument.");
                Environment.Exit(1);
            }
            return ipAddress;
        }


        static void Main(string[] args)
        {
            Console.Write("mode: ");
            string mode = Console.ReadLine().Trim();

            if (mode == "server")
            {
                ProcessServer(ReadPort());
            } else if (mode == "client")
            {
                TcpClient tcpClient = new TcpClient();
                var endpoint = new IPEndPoint(ReadIP(), ReadPort());
                tcpClient.Connect(endpoint);
                Console.WriteLine("Connected to " + endpoint);
                ProcessClient(tcpClient);
            }


        }
    }
}
