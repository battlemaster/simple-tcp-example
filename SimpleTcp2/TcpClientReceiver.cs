using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

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
                    Console.WriteLine("Loaded {0} bytes", readCount);
                    int length = BitConverter.ToInt32(buffer, 0);
                    int offset = 4;
                    int filenameEnds = 4;
                    for (; filenameEnds < buffer.Length && buffer[filenameEnds] != 0; filenameEnds++) ;
                    if (filenameEnds == buffer.Length)
                    {
                        Console.WriteLine("Incorrect message structure");
                    }
                    string filename = ASCIIEncoding.ASCII.GetString(buffer, offset, filenameEnds - offset);
                    offset = filenameEnds + 1;
                    var dirName = @"loaded\";
                    if (!Directory.Exists(dirName))
                    {
                        Directory.CreateDirectory(dirName);
                    }

                    using (FileStream fs = new FileStream(dirName + filename, FileMode.Create)) 
                    {
                        int fileBytesWritten = 0;
                        do
                        {
                            if (offset < readCount)
                            {
                                fs.Write(buffer, offset, readCount - offset);
                                fileBytesWritten += readCount - offset;
                            }
                            if (fileBytesWritten < length)
                            {
                                offset = 0;
                                readCount = stream.Read(buffer, 0, Math.Min(buffer.Length, length - fileBytesWritten));
                                Console.WriteLine("Loaded {0} bytes", readCount);
                            }

                        } while (fileBytesWritten < length);
                        Console.WriteLine("File with name {0} was written", filename);
                    }
                   
                }
            } catch (Exception e)
            {
                Console.WriteLine("received stopped due error: {0}", e.Message);
            }
    }

    }
}
