﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;

namespace Client
{
    public class UDPClient
    {
        private readonly string server;
        private readonly int port;

        public UDPClient(string server, int port)
        {
            this.server = server;
            this.port = port;
        }

        public void SendMessage(int messageSize, int totalBytesToSend, bool useStopAndWait)
        {
            using var client = new UdpClient();
            var serverEndPoint = new IPEndPoint(IPAddress.Parse(server), port);

            byte[] message = new byte[messageSize];
            new Random().NextBytes(message);

            int bytesSent = 0;
            int messagesSent = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (bytesSent < totalBytesToSend)
            {
                client.Send(message, message.Length, serverEndPoint);
                bytesSent += message.Length;
                messagesSent++;

                if (useStopAndWait)
                {
                    // Implement logic for stop-and-wait acknowledgement
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"From Client - Protocol: TCP, Transmission time: {stopwatch.ElapsedMilliseconds} ms, Messages sent: {messagesSent}, Bytes sent: {bytesSent}");
        }
    }
}
