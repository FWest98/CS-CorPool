using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace CorPool.HubTest {
    class Program {
        static async Task Main(string[] args) {
            var urls = new List<string> {
                "http://shell.192.168.10.7.xip.io:33080/",
                "https://shell.staging.corpool.nl/"
            };
            Console.WriteLine("Choose a server to connect to:");
            for (var i = 0; i < urls.Count; i++) {
                Console.WriteLine($"{i}: {urls[i]}");
            }

            if (!int.TryParse(Console.ReadLine(), out var index)) {
                return;
            }

            var url = urls[index];

            Console.WriteLine("Enter your authorization key:");
            var token = Console.ReadLine()?.Trim();

            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"{url}/api/ride/find",
                    HttpTransportType.WebSockets,
                    s => {
                        s.Headers.Add("Authorization", $"Bearer {token}");
                        s.SkipNegotiation = true;
                    })
                .WithAutomaticReconnect()
                .AddJsonProtocol()
                .Build();

            await hubConnection.StartAsync();

            Console.WriteLine("Enter text to send to RabbitEcho (empty for exit)");

            hubConnection.On<string>("echo", Console.WriteLine);

            string input;
            while ((input = Console.ReadLine()) != "") {
                // Echo Input
                await hubConnection.SendAsync("RabbitEcho", input);
                //Console.WriteLine(res);
            }
        }
    }
}
