using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace DataTransferServer
{
    internal class UDPServer
    {
        static void Main(string[] args)
        {
            const int listenPort = 11000;

            using (var udpClient = new UdpClient(listenPort))
            {
                var remoteEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
                Console.WriteLine("UDP Server listening on port " + listenPort);

                try
                {
                    while (true)
                    {
                        Console.WriteLine("Waiting for a message...");
                        byte[] receivedBytes = udpClient.Receive(ref remoteEndPoint);
                        string receivedMessage = Encoding.ASCII.GetString(receivedBytes);

                        Console.WriteLine($"Received: {receivedMessage} from {remoteEndPoint}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
