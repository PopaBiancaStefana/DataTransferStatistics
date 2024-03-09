using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace DataTransferClient
{
    internal class UDPClient
    {
        static void Main(string[] args)
        {
            const string server = "127.0.0.1";
            const int port = 11000;

            var serverEndPoint = new IPEndPoint(IPAddress.Parse(server), port);
            using (var udpClient = new UdpClient())
            {
                try
                {
                    udpClient.Connect(serverEndPoint);
                    string message = "Hello from UDP client";
                    byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                    udpClient.Send(sendBytes, sendBytes.Length);
                    Console.WriteLine($"Message sent to the server: {message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
