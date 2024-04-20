using System.Net;
using System.Text;

namespace UdpClient;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            const int serverPort = 8080;
            
            // Parse the port number from the command-line arguments
            var port = args.Length > 0 ? int.Parse(args[0]) : 8081;
            var userId = args.Length > 0 ? int.Parse(args[1]) : 0;
            
            
            using var udpClient = new System.Net.Sockets.UdpClient(port);
            // Set up the server endpoint (replace with actual server details)
            var serverIp = IPAddress.Parse("127.0.0.1");
            var serverEndPoint = new IPEndPoint(serverIp, serverPort);

            // Send an initial message to the server
            var initialMessageBytes = Encoding.UTF8.GetBytes(userId.ToString());
            await udpClient.SendAsync(initialMessageBytes, initialMessageBytes.Length, serverEndPoint);

            // Continuously listen for incoming messages
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var receivedResult = await udpClient.ReceiveAsync();
                    var receivedMessage = Encoding.UTF8.GetString(receivedResult.Buffer);
                    Console.WriteLine(
                        $"Received from server: {receivedMessage}\nEnter a message to send to the server: ");
                }
            });

            // Get input from user and send to the server
            while (true)
            {
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