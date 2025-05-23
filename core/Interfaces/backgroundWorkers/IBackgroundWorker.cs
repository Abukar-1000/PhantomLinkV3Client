

using Microsoft.AspNetCore.SignalR.Client;

namespace BackgrounderWorker
{
    public interface IBackgroundWorker
    {
        public Task<HubConnection?> ConnectToControlHub();
        public Task<HubConnection?> ConnectToHub();
        public Task<bool> IsRegistered();
    }
}