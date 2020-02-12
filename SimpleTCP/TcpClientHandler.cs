using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SimpleTCP
{
    class TcpClientHandler
    {

        public void Run(TcpClient tcpClient)
        {
            try
            {
                string message = null;
                var stream = tcpClient.GetStream();
                byte[] buffer = new byte[512];
                do
                {
                    message = Console.ReadLine();
                    if (message != "q")
                    {
                        var written = ASCIIEncoding.ASCII.GetBytes(message, 0, message.Length, buffer, 0);
                        stream.Write(buffer, 0, written);
                    }
                }
                while (message != "q");
            } finally
            {
                tcpClient.Close();
            }
        }

    }
}
