using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using BackgrounderWorker;
using DeviceSpace;
using SocketRoutes;
using ProcessSpace;

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

        ProcessPool pool = new();
        // pool.ViewAllHash();
        await BackgroundWorkers.StartProcessWorker();
        
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
