using DeviceSpace;
using Microsoft.AspNetCore.SignalR.Client;
using ProcessSpace;
using ProcessSpace.Models;
using Microsoft.AspNetCore.Http;

namespace BackgrounderWorker {
    public class ProcessWorker : BackgroundWorkerBase
    {
        protected readonly Dictionary<string, ProcessSnapshot> _lookUp;
        protected readonly ProcessWorkerParams _params;
        protected readonly Device _device;

        public ProcessWorker()
        {
            _lookUp = new();
            _device = new();
        }

        public ProcessWorker(
            ProcessWorkerParams parameters,
            Device device
        )
            : base(parameters, device)
        {
            _lookUp = new();
            _device = new();
            _params = parameters;
        }

        protected bool HasChanged(ProcessSnapshot snapshot)
        {
            bool isNewGroup = _lookUp.ContainsKey(snapshot.processName) == false;
            if (isNewGroup)
            {
                return true;
            }

            ProcessSnapshot previousSnapshot = _lookUp[snapshot.processName];
            bool activeStateChanged = snapshot.status != previousSnapshot.status;
            return activeStateChanged;
        }

        protected List<ProcessSnapshot>? CheckForDeadProcesses(ProcessPool monitor)
        {
            List<ProcessSnapshot> deadChanges = new();

            foreach (var pair in monitor.GetCurrentShot())
            {
                string processGroup = pair.Key;
                string currentGroupHash = pair.Value.Hash;
                var group = pair.Value._group;

                bool isDead = group.All(process => process.IsRunning == false);
                if (isDead)
                {
                    ProcessSnapshot snapshot = new ProcessSnapshot(
                        processGroup,
                        ProcessStatus.Dead,
                        pair.Value,
                        _device.id
                    );

                    bool isNewChange = this.HasChanged(snapshot);
                    if (isNewChange)
                    {
                        deadChanges.Add(snapshot);
                        _lookUp[snapshot.processName] = snapshot;
                    }
                }
            }

            if (deadChanges.Count == 0)
            {
                return null;
            }

            return deadChanges;
        }

        protected List<ProcessSnapshot>? CheckForNewProcesses(ProcessPool monitor)
        {
            List<ProcessSnapshot> newChanges = new();

            foreach (var pair in monitor.GetCurrentShot())
            {
                string processGroup = pair.Key;
                string currentGroupHash = pair.Value.Hash;
                var group = pair.Value._group;

                bool isActive = group.Any(process => process.IsRunning);
                if (isActive)
                {
                    ProcessSnapshot snapshot = new ProcessSnapshot(
                        processGroup,
                        ProcessStatus.Alive,
                        pair.Value,
                        _device.id
                    );

                    bool isNewChange = this.HasChanged(snapshot);
                    if (isNewChange)
                    {
                        newChanges.Add(snapshot);
                        _lookUp[snapshot.processName] = snapshot;
                    }
                }
            }

            if (newChanges.Count == 0)
            {
                return null;
            }

            return newChanges;
        }

        public async Task StartProcessMonitor()
        {
            const int delay = 10;
            ProcessPool monitor = new();
            var _connection = await ConnectToHub();
            bool isRegistered = await IsRegistered();

            _connection?.On<string>("ProcessUpdateResponse", (message) =>
            {
                Console.WriteLine($"Got: {message}");
            });

            _connection?.On<ProcessKillFrame>("ProcessKillRequest", async (frame) =>
            {
                Console.WriteLine($"Kill request Got: {frame.processName}");
                ProcessPool killMonitor = new();
                await KillProcess(frame, killMonitor, _connection);
            });

            while (true)
            {

                if (_params.running)
                {
                    monitor.Update();
                    List<ProcessSnapshot>? newProcesses = this.CheckForNewProcesses(monitor);
                    List<ProcessSnapshot>? deadProcesses = this.CheckForDeadProcesses(monitor);

                    if (newProcesses is not null || deadProcesses is not null)
                    {
                        Console.WriteLine($"\n\n\n");
                    }

                    if (newProcesses is not null)
                    {
                        foreach (ProcessSnapshot processSnapshot in newProcesses)
                        {
                            await _connection?.InvokeAsync("UpdateProcess", new ProcessUpdateFrame(processSnapshot));
                        }
                    }

                    if (deadProcesses is not null)
                    {
                        foreach (ProcessSnapshot processSnapshot in deadProcesses)
                        {
                            await _connection?.InvokeAsync("UpdateProcess", new ProcessUpdateFrame(processSnapshot));
                        }
                    }

                }
                await Task.Delay(delay);
            }
        }

        protected async Task KillProcess(
            ProcessKillFrame frame,
            ProcessPool pool,
            HubConnection? connection
        )
        {
            var snapshot = pool.GetCurrentShot();

            // handle process no longer exists
            ExecutionStatus executionStatus = pool.Kill(frame.processName);
            int status = executionStatus == ExecutionStatus.Success ?
                                                StatusCodes.Status200OK :
                                                StatusCodes.Status500InternalServerError;

            ProcessKillFrameResponse response = new ProcessKillFrameResponse(frame, status);
            await connection?.InvokeAsync("KillProcessResponse", response);
        }
        
    }
}