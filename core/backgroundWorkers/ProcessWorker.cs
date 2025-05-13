using DeviceSpace;
using Microsoft.AspNetCore.SignalR.Client;
using ProcessSpace;
using ProcessSpace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using SocketRoutes;
using System.Net;

namespace BackgrounderWorker {
    public class ProcessWorker {
        protected readonly Dictionary<string, ProcessSnapshot> _lookUp;
        protected readonly ProcessWorkerParams _params;
        protected readonly Device _device;

        public ProcessWorker() {
            _lookUp = new();
            _device = new();
        }

        public ProcessWorker(ProcessWorkerParams parameters) {
            _lookUp = new();
            _device = new();
            _params = parameters;
        }

        protected bool HasChanged(ProcessSnapshot snapshot) {
            bool isNewGroup = _lookUp.ContainsKey(snapshot.processName) == false;
            if (isNewGroup) {
                return true;
            }

            ProcessSnapshot previousSnapshot = _lookUp[snapshot.processName];
            bool activeStateChanged = snapshot.status != previousSnapshot.status;
            return activeStateChanged;
        }

        protected List<ProcessSnapshot>? CheckForDeadProcesses(ProcessPool monitor) {
            List<ProcessSnapshot> deadChanges = new();
            
            foreach (var pair in monitor.GetCurrentShot()) {
                string processGroup = pair.Key;
                string currentGroupHash = pair.Value.Hash;
                var group = pair.Value._group;

                bool isDead = group.All(process => process.IsRunning == false);
                if (isDead) {
                    ProcessSnapshot snapshot = new ProcessSnapshot(
                        processGroup,
                        ProcessStatus.Dead,
                        pair.Value,
                        _device.id
                    );

                    bool isNewChange = this.HasChanged(snapshot);
                    if (isNewChange) {
                        deadChanges.Add(snapshot);
                        _lookUp[snapshot.processName] = snapshot;
                    }
                }
            }

            if (deadChanges.Count == 0) {
                return null;
            }

            return deadChanges;
        }

        protected List<ProcessSnapshot>? CheckForNewProcesses(ProcessPool monitor) {
            List<ProcessSnapshot> newChanges = new();

            foreach (var pair in monitor.GetCurrentShot()) {
                string processGroup = pair.Key;
                string currentGroupHash = pair.Value.Hash;
                var group = pair.Value._group;

                bool isActive = group.Any(process => process.IsRunning);
                if (isActive) {
                    ProcessSnapshot snapshot = new ProcessSnapshot(
                        processGroup,
                        ProcessStatus.Alive,
                        pair.Value,
                        _device.id
                    );

                    bool isNewChange = this.HasChanged(snapshot);
                    if (isNewChange) {
                        newChanges.Add(snapshot);
                        _lookUp[snapshot.processName] = snapshot;
                    }
                }
            }
            
            if (newChanges.Count == 0) {
                return null;
            }

            return newChanges;
        }

        public async Task StartProcessMonitor() {
            const int delay = 10;
            ProcessPool monitor = new();
            var _connection = await ConnectToHub();
            bool isRegistered = await IsRegistered();

            _connection?.On<string>("ProcessUpdateResponse", (message) =>
            {
                Console.WriteLine($"Got: {message}");
            });

            while (true) {

                if (_params.running) {
                    monitor.Update();
                    List<ProcessSnapshot>? newProcesses = this.CheckForNewProcesses(monitor);
                    List<ProcessSnapshot>? deadProcesses = this.CheckForDeadProcesses(monitor);

                    if (newProcesses is not null || deadProcesses is not null) {
                        Console.WriteLine($"\n\n\n");
                    }

                    if (newProcesses is not null) {
                        foreach (ProcessSnapshot processSnapshot in newProcesses) {
                            await _connection?.InvokeAsync("UpdateProcess", new ProcessUpdateFrame(processSnapshot));
                            // Console.WriteLine($"[+]ID:\t{processSnapshot.deviceID} \tName:\t{processSnapshot.processName} Active");
                        }
                    }

                    if (deadProcesses is not null) {
                        foreach (ProcessSnapshot processSnapshot in deadProcesses) {
                            await _connection?.InvokeAsync("UpdateProcess", new ProcessUpdateFrame(processSnapshot));
                            // Console.WriteLine($"[-]ID:\t{processSnapshot.deviceID}\tName:\t{processSnapshot.processName} Inactive");
                        }
                    }
                    
                }
                await Task.Delay(delay);
            }
        }

        protected async Task<bool> IsRegistered() {
            bool registered = false;
            var _connection = await ConnectToControlHub();
            await _connection?.InvokeAsync("RegisterDevice", new Device());

            _connection?.On<int>("RegisterResponse", (status) =>
            {
                Console.WriteLine($"Register response:\t{status}");
                registered = status == StatusCodes.Status200OK || status == StatusCodes.Status201Created;
            });

            return registered;
        }

        protected async Task<HubConnection?> ConnectToControlHub() {
            Console.WriteLine($"Connecting to hub {_params.route}");
            var hub = new HubConnectionBuilder()
                        .WithUrl(_params.controlRoute)
                        .WithAutomaticReconnect()
                        .Build();
            try {
                await hub.StartAsync();
                return hub;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }

            return null;
        }

        protected async Task<HubConnection?> ConnectToHub() {
            Console.WriteLine($"Connecting to hub {_params.route}");
            var hub = new HubConnectionBuilder()
                        .WithUrl(_params.route)
                        .WithAutomaticReconnect()
                        .Build();
            try {
                await hub.StartAsync();
                return hub;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }

            return null;
        }
    }
}