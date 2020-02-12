using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SimpleTCP
{
    class TcpClientReceiver
    {

        private readonly TcpClient tcpClient;

        public TcpClientReceiver(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public void Run()
        {
            try
            {
                var stream = tcpClient.GetStream();
                byte[] buffer = new byte[512];
                while (true)
                {
                    int readCount = stream.Read(buffer, 0, buffer.Length);
                    if (readCount > 0)
                    {
                        string response = ASCIIEncoding.ASCII.GetString(buffer, 0, readCount);
                        Console.WriteLine("received message: {0}", response);
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("received stopped due error: {0}", e.Message);
            }
    }

    }
}
