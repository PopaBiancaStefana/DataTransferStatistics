using System.Net;
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

        public async Task SendMessage(int messageSize, int totalBytesToSend, bool useStopAndWait)
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
                await client.SendAsync(message, message.Length, serverEndPoint);
                bytesSent += message.Length;
                messagesSent++;

                if (useStopAndWait)
                {
                    UdpReceiveResult receivedResult = await client.ReceiveAsync();
                    string ack = Encoding.UTF8.GetString(receivedResult.Buffer);
                    if (ack != "ACK")
                    {
                        throw new Exception("ACK not received or incorrect.");
                    }
                }
            }

            stopwatch.Stop();
            var mechanismUsed = useStopAndWait ? "Stop-and-Wait" : "Streaming";
            Console.WriteLine($"From Client - Protocol: UDP, Transmission time: {stopwatch.ElapsedMilliseconds} ms, Messages sent: {messagesSent}, Bytes sent: {bytesSent}, Mechanism used: {mechanismUsed}");
        }
    }
}
