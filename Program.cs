using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using BackgrounderWorker;
using DeviceSpace;
using SocketRoutes;
using ProcessSpace;
/*
    - Limit frame creation in Screen broadcast => should limit ram usage
    - Implement mouse movement
    - Implement key logger => encapsulate in KeyboardBackgroundWorker 
    - Implement background usage controles => on/off
    - Add CPU, Memory, Disk, Network, GPU Usage broadcast => All encapsed in Hardware
*/
class Program
{
    static async Task Main(string[] args)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl(Routes.control)
            .WithAutomaticReconnect()
            .Build();

        // Receive a message from the server
        connection.On<int>("RegisterResponse", (message) =>
        {
            Console.WriteLine($"Got: {message}");
        });

        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connection started.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
        }

        BackgroundWorkers workers = new BackgroundWorkers();
        workers.StartProcessWorker();
        await workers.StartScreenMonitor();
        
        // Send a message to the server
        Console.WriteLine("Type messages to send. Type 'exit' to quit.");
        while (true)
        {
            var message = Console.ReadLine();
            if (message?.ToLower() == "exit") break;

            await connection.InvokeAsync("RegisterDevice", new Device());
        }

        await connection.StopAsync();
    }
}
