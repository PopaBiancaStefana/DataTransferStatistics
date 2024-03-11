using Client;
using Server;

namespace DataTransferS
{
    internal class DataTransfer
    {
        public static async Task Main(string[] args)
        {
            DataTransfer data = new DataTransfer();

            await data.getTCPWithStreamingStatistics();

            await Task.Delay(5000);
            await data.getTCPWithStopAndWaitStatistics();
        }

        private async Task getTCPWithStreamingStatistics()
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

        private async Task getTCPWithStopAndWaitStatistics()
        {
            var tcpServer = new TCPServer(port: 11000);
            var tcpClient = new TCPClient(server: "127.0.0.1", port: 11000);
            tcpServer.UseStopAndWait = true;

            var serverTask = tcpServer.Run();
            await Task.Delay(1000);


            // Sending 1 MB = 1048576 bytes, 500 MB = 524288000 bytes and 1 GB = 1073741824 bytes
            await tcpClient.SendMessage(messageSize: 65535, totalBytesToSend: 1048576, useStopAndWait: true);
            await tcpClient.SendMessage(messageSize: 65535, totalBytesToSend: 524288000, useStopAndWait: true);
            await tcpClient.SendMessage(messageSize: 65535, totalBytesToSend: 1073741824, useStopAndWait: true);

            await Task.Delay(1000);
            tcpServer.Stop();
            await serverTask;
        }
    }
}
