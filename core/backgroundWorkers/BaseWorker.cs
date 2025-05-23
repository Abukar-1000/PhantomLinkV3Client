


using DeviceSpace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using ProcessSpace;
using Register.Models;

namespace BackgrounderWorker
{
    public abstract class BackgroundWorkerBase : IBackgroundWorker
    {
        protected IBackgroundWorkerParams _params;
        protected IDevice _device;
        public BackgroundWorkerBase()
        {
            _params = null;
        }

        public BackgroundWorkerBase(IBackgroundWorkerParams _params)
        {
            this._params = _params;
        }

        public BackgroundWorkerBase(
            IBackgroundWorkerParams _params,
            IDevice _device
        )
        {
            this._params = _params;
            this._device = _device;
        }

        public async Task<HubConnection?> ConnectToControlHub()
        {
            Console.WriteLine($"Connecting to hub {_params.route}");
            var hub = new HubConnectionBuilder()
                        .WithUrl(_params.controlRoute)
                        .WithAutomaticReconnect()
                        .Build();
            try
            {
                await hub.StartAsync();
                return hub;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }

            return null;
        }

        public async Task<HubConnection?> ConnectToHub()
        {
            Console.WriteLine($"Connecting to hub {_params.route}");
            var hub = new HubConnectionBuilder()
                        .WithUrl(_params.route)
                        .WithAutomaticReconnect()
                        .Build();
            try
            {
                await hub.StartAsync();
                return hub;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }

            return null;
        }
        
        public async Task<bool> IsRegistered() {
            bool registered = false;
            var _connection = await ConnectToControlHub();

            RegisterFrame _device = this._device.GetData(); 
            await _connection?.InvokeAsync("RegisterDevice", _device);

            _connection?.On<int>("RegisterResponse", (status) =>
            {
                Console.WriteLine($"Register response:\t{status}");
                registered = status == StatusCodes.Status200OK || status == StatusCodes.Status201Created;
            });

            return registered;
        }
    }

}