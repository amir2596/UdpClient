using System.Net;
using System.Text;

namespace UdpClient;

internal static class Program
{
    private static async Task Main()
    {
        try
        {
            using var udpClient = new System.Net.Sockets.UdpClient();
            // Set up the server endpoint (replace with actual server details)
            var serverIp = IPAddress.Parse("127.0.0.1");
            const int serverPort = 8080;
            var serverEndPoint = new IPEndPoint(serverIp, serverPort);

            // Send an initial message to the server
            const string initialMessage = "Hello, Netty server!";
            var initialMessageBytes = Encoding.UTF8.GetBytes(initialMessage);
            await udpClient.SendAsync(initialMessageBytes, initialMessageBytes.Length, serverEndPoint);

            // Continuously listen for incoming messages
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var receivedResult = await udpClient.ReceiveAsync();
                    var receivedMessage = Encoding.UTF8.GetString(receivedResult.Buffer);
                    Console.WriteLine($"Received from server: {receivedMessage}");
                }
            });

            // Get input from user and send to the server
            while (true)
            {
                Console.Write("Enter a message to send to the server: ");
                var userInput = Console.ReadLine();
                var userMessageBytes = Encoding.UTF8.GetBytes(userInput);
                await udpClient.SendAsync(userMessageBytes, userMessageBytes.Length, serverEndPoint);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}