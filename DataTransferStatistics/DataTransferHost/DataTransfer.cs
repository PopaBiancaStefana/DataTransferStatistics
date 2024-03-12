using Client;
using Server;

namespace DataTransferS
{
    internal class DataTransfer
    {
        public static async Task Main(string[] args)
        {
            DataTransfer data = new DataTransfer();


            //Console.WriteLine("Testing TCP protocol, single client,various messages sizes, streaming vs stop and await mechanisms");
            //// Sending 1 MB = 1048576 bytes, 500 MB = 524288000 bytes and 1 GB = 1073741824 bytes
            //await data.TCPWithStreamingSingleClient();
            //await data.TCPWithStopAndWaitSingleClient();

            //Console.WriteLine("\nTesting TCP protocol, multiple clients, streaming vs stop and await mechanisms");
            //// Sending 1 MB
            //await data.TCPWithStreamingMultipleClients();
            //await data.TCPWithStopAndAwaitMultipleClients();

            //Console.WriteLine("\nTesting UDP protocol, single client, various messages sizes, streaming vs stop and await mechanisms");
            //// Sending 1 MB = 1048576 bytes, 500 MB = 524288000 bytes and 1 GB = 1073741824 bytes
            //await data.UDPWithStreamingSingleClient();
            //await data.UDPWithStopAndWaitSingleClient();

            Console.WriteLine("\nTesting UDP protocol, multiple clients, streaming vs stop and await mechanisms");
            // Sending 1 MB
            await data.UDPWithStreamingMultipleClients();
            await data.UDPWithStopAndAwaitMultipleClients();

        }

        private async Task TCPWithStreamingSingleClient()
        {
            var tcpServer = new TCPServer(port: 11000);
            var tcpClient = new TCPClient(server: "127.0.0.1", port: 11000);

            var serverTask = tcpServer.Run();
            await Task.Delay(1000);

            // Sending 1 MB = 1048576 bytes, 500 MB = 524288000 bytes and 1 GB = 1073741824 bytes
            await tcpClient.SendMessage(messageSize: 65535, totalBytesToSend: 1048576, useStopAndWait: false);
            await tcpClient.SendMessage(messageSize: 65535, totalBytesToSend: 524288000, useStopAndWait: false);
            await tcpClient.SendMessage(messageSize: 65535, totalBytesToSend: 1073741824, useStopAndWait: false);

            await Task.Delay(1000);
            tcpServer.Stop();
            await serverTask;
        }

        private async Task TCPWithStopAndWaitSingleClient()
        {
            var tcpServer = new TCPServer(port: 11000);
            var tcpClient = new TCPClient(server: "127.0.0.1", port: 11000);
            tcpServer.UseStopAndWait = true;

            var serverTask = tcpServer.Run();
            await Task.Delay(1000);

            await tcpClient.SendMessage(messageSize: 65535, totalBytesToSend: 1048576, useStopAndWait: false);
            await tcpClient.SendMessage(messageSize: 65535, totalBytesToSend: 524288000, useStopAndWait: false);
            await tcpClient.SendMessage(messageSize: 65535, totalBytesToSend: 1073741824, useStopAndWait: false);

            await Task.Delay(1000);
            tcpServer.Stop();
            await serverTask;
        }

        private async Task TCPWithStreamingMultipleClients()
        {
            var tcpServer = new TCPServer(port: 11000);
            var serverTask = tcpServer.Run();

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var client = new TCPClient("127.0.0.1", 11000);
                // Sending messages concurrently, without awaiting
                var task = client.SendMessage(messageSize: 65535, totalBytesToSend: 1048576, useStopAndWait: false);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            await Task.Delay(1000);
            tcpServer.Stop();
            await serverTask;
        }

        private async Task TCPWithStopAndAwaitMultipleClients()
        {
            var tcpServer = new TCPServer(port: 11000);
            tcpServer.UseStopAndWait = true;
            var serverTask = tcpServer.Run();

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var client = new TCPClient("127.0.0.1", 11000);
                // Sending messages concurrently, without awaiting
                var task = client.SendMessage(messageSize: 65535, totalBytesToSend: 1048576, useStopAndWait: true);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            await Task.Delay(1000);
            tcpServer.Stop();
            await serverTask;
        }

        private async Task UDPWithStreamingSingleClient()
        {
            var udpServer = new UDPServer(port: 11000);
            var udpClient = new UDPClient(server: "127.0.0.1", port: 11000);

            var serverTask = udpServer.Run();
            await Task.Delay(1000);

            await udpClient.SendMessage(messageSize: 16384, totalBytesToSend: 1048576, useStopAndWait: false);
            await udpClient.SendMessage(messageSize: 16384, totalBytesToSend: 524288000, useStopAndWait: false);
            await udpClient.SendMessage(messageSize: 16384, totalBytesToSend: 1073741824, useStopAndWait: false);

            udpServer.Stop();
            await serverTask;
        }

        private async Task UDPWithStopAndWaitSingleClient()
        {
            var udpServer = new UDPServer(port: 11000);
            var udpClient = new UDPClient(server: "127.0.0.1", port: 11000);
            udpServer.UseStopAndWait = true;

            var serverTask = udpServer.Run();
            await Task.Delay(1000);

            await udpClient.SendMessage(messageSize: 16384, totalBytesToSend: 1048576, useStopAndWait: true);
            await udpClient.SendMessage(messageSize: 16384, totalBytesToSend: 524288000, useStopAndWait: true);
            await udpClient.SendMessage(messageSize: 16384, totalBytesToSend: 1073741824, useStopAndWait: true);

            udpServer.Stop();
            await serverTask;
        }

        private async Task UDPWithStreamingMultipleClients()
        {
            var udpServer = new UDPServer(port: 11000);
            var serverTask = udpServer.Run();

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var client = new UDPClient("127.0.0.1", 11000);
                var task = client.SendMessage(messageSize: 16384, totalBytesToSend: 1048576, useStopAndWait: false);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            await Task.Delay(1000);
            udpServer.Stop();
            await serverTask;
        }

        private async Task UDPWithStopAndAwaitMultipleClients()
        {
            var udpServer = new UDPServer(port: 11000);
            udpServer.UseStopAndWait = true;
            var serverTask = udpServer.Run();

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var client = new UDPClient("127.0.0.1", 11000);
                var task = client.SendMessage(messageSize: 16384, totalBytesToSend: 1048576, useStopAndWait: true);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            await Task.Delay(1000);
            udpServer.Stop();
            await serverTask;
        }
    }
}
