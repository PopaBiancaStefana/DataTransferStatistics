using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Diagnostics;

namespace Client
{
    public class TCPClient
    {
        private readonly string server;
        private readonly int port;

        public TCPClient(string server, int port)
        {
            this.server = server;
            this.port = port;
        }
        public async Task SendMessage(int messageSize, int totalBytesToSend, bool useStopAndWait)
        {
            using var client = new TcpClient();

            await client.ConnectAsync(server, port);
            var networkStream = client.GetStream();

            byte[] message = new byte[messageSize];
            new Random().NextBytes(message);

            int bytesSent = 0;
            int messagesSent = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (bytesSent < totalBytesToSend)
            {
                await networkStream.WriteAsync(message, 0, message.Length);
                bytesSent += message.Length;
                messagesSent++;
            }

            stopwatch.Stop();
            var mechanismUsed = useStopAndWait ? "Stop-and-Wait" : "Streaming";
            Console.WriteLine($"From Client - Transmission time: {stopwatch.ElapsedMilliseconds} ms, Messages sent: {messagesSent}, Bytes sent: {bytesSent}, Mechanism used: {mechanismUsed}");
        }
    }
}
