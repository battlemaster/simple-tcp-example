using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

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
                    var info = new System.IO.FileInfo(message);
                    var filename = Path.GetFileName(message);

                    if (info.Exists)
                    {
                        using (System.IO.FileStream fs = new System.IO.FileStream(message, System.IO.FileMode.Open))
                        {
                            var length = info.Length;
                            int sentCount = 0;
                            //add file length to buffer
                            Array.Copy(BitConverter.GetBytes(length), 0, buffer, 0, 4);
                            int ind = 4;
                            //add filename to buffer (separated by 0 byte)
                            ind += ASCIIEncoding.ASCII.GetBytes(filename, 0, filename.Length, buffer, 4);
                            buffer[ind] = 0;
                            ind++;
                            while (sentCount != length)
                            {
                                var read = fs.Read(buffer, ind, buffer.Length - ind);
                                stream.Write(buffer, 0, read + ind);
                                Console.WriteLine("Sent {0} bytes", read + ind);
                                ind = 0;
                                sentCount += read;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("File is not found");
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
