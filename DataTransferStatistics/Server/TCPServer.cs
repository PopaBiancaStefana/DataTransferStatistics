using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    public class TCPServer(int port)
    {
        private readonly int port = port;
        private bool isRunning = false;
        private TcpListener? listener;

        public bool UseStopAndWait { get; set; } = false;

        public async Task Run()
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"TCP Server listening on port {port}...");
            isRunning = true;

            try
            {
                while (isRunning)
                {
                    var clientTask = listener.AcceptTcpClientAsync();
                    if (clientTask == await Task.WhenAny(clientTask, Task.Delay(Timeout.Infinite)))
                    {
                        var client = await clientTask;
                        _ = HandleClientAsync(client);
                    }
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Server stopped.");
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            int totalBytesReceived = 0;
            int totalMessagesReceived = 0;
            try
            {
                using var nerworkStream = client.GetStream();
                var buffer = new byte[65535];
                int bytesRead;

                while ((bytesRead = await nerworkStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    totalBytesReceived += bytesRead;
                    totalMessagesReceived++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                client.Close();
            }
            var mechanismUsed = UseStopAndWait ? "Stop-and-Wait" : "Streaming";
            Console.WriteLine($"From Server - Protocol: TCP, Messages read: {totalMessagesReceived}, Bytes read: {totalBytesReceived}, Mechanism used: {mechanismUsed}\n");
        }

        public void Stop()
        {
            isRunning = false;
            listener?.Stop();
        }
    }
}
