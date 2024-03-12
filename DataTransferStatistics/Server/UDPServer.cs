using System.Net;
using System.Text;
using System.Net.Sockets;

namespace Server
{
    public class UDPServer
    {
        private readonly int port;
        private readonly CancellationTokenSource cancellationTokenSource;
        private bool isRunning = false;
        public bool UseStopAndWait { get; set; } = false;

        public UDPServer(int port)
        {
            this.port = port;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task Run()
        {
            using var udpClient = new UdpClient(port);

            Console.WriteLine($"UDP Server listening on port {port}...");
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

            int totalBytesReceived = 0;
            int totalMessagesReceived = 0;
            isRunning = true;

            try
            {
                while (isRunning)
                {
                    if (udpClient.Client == null) break;

                    var receiveTask = udpClient.ReceiveAsync();
                    await Task.WhenAny(receiveTask, Task.Delay(Timeout.Infinite, cancellationTokenSource.Token));

                    if (receiveTask.IsCompleted)
                    {
                        var receivedData = receiveTask.Result;
                        totalBytesReceived += receivedData.Buffer.Length;
                        totalMessagesReceived++;

                        if (UseStopAndWait)
                        {
                            string ack = "ACK";
                            byte[] ackBytes = Encoding.UTF8.GetBytes(ack);
                            await udpClient.SendAsync(ackBytes, ackBytes.Length, receivedData.RemoteEndPoint);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("UDP Server stopped.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var mechanismUsed = UseStopAndWait ? "Stop-and-Wait" : "Streaming";
            Console.WriteLine($"From Server - Protocol: UDP, Messages read: {totalMessagesReceived}, Bytes read: {totalBytesReceived}, Mechanism used: {mechanismUsed}");
        }

        public void Stop()
        {
            isRunning = false;
            cancellationTokenSource.Cancel();
            Console.WriteLine("UDP Server stopped.\n");
        }
    }
}
