using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Server
{
    public class UDPServer
    {
        private readonly int port;
        private bool isRunning = false;

        public UDPServer(int port)
        {
            this.port = port;
        }

        public void Run(bool useStopAndWait)
        {
            using var server = new UdpClient(port);

            Console.WriteLine($"UDP Server listening on port {port}...");
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            int totalBytesReceived = 0;
            int totalMessagesReceived = 0;
            isRunning = true;

            try
            {
                while (isRunning)
                {
                    byte[] receivedData = server.Receive(ref clientEndPoint);
                    totalBytesReceived += receivedData.Length;
                    totalMessagesReceived++;

                    // Optionally send acknowledgment for stop-and-wait mechanism
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine($"From Server - Protocol: UDP, Messages read: {totalMessagesReceived}, Bytes read: {totalBytesReceived}");
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}
