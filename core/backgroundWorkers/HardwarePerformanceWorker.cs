using BackgrounderWorker;
using DeviceSpace;
using HardwarePerformance.Models;
using HardwareSpace.HardwarePerformance.Options;
using Microsoft.AspNetCore.SignalR.Client;
using Register.Models;
using System.Diagnostics;
using HardwareBackground = HardwareSpace.BackgrounderWorker;
using HardwarePerformanceOptions = HardwareSpace.HardwarePerformance.Options;

namespace HardwareSpace
{
    public class HardwarePerformanceWorker : BackgroundWorkerBase
    {
        protected IBackgroundWorkerParams _params;
        protected IDevice _device;

        public HardwarePerformanceWorker() { }
        public HardwarePerformanceWorker(
            HardwareBackground.HardwarePerformanceParams _params,
            Device device
        )
            : base(_params, device)
        {
            this._params = _params;
            this._device = device;
        }

        public async Task StartPerformanceMonitor()
        {
            var _connection = await ConnectToHub();
            bool isRegistered = await IsRegistered();
            RegisterFrame registerFrame = _device.GetData();

            var cpuCounter = this.GetCPUMonitor();
            var ramCounter = this.GetMemoryMonitor();
            var gpuCounter = this.GetGPUMonitor();
            var heartbeat = new HeartbeatFrame();
            heartbeat.id = registerFrame.id;

            _connection?.On<HeartbeatFrame>("HeartbeatFrameResponse", async (frame) =>
            {
                Console.WriteLine($"[Performance Monitor]. \tCPU: {frame.CPU.value} \tMEM: {frame.Memory.value} \tGPU: {frame.GPU.value}");
            });

            while (true)
            {
                if (_params.running)
                {
                    heartbeat.Configure(
                        cpuCounter.Next(),
                        gpuCounter.Next(),
                        ramCounter.Next()
                    );

                    await _connection?.InvokeAsync("BrodcastPerformance", heartbeat);
                    await Task.Delay(500);
                }

                if (_params.running is false)
                {
                    await Task.Delay(100);
                }
            }
        }

        protected CPUHardwarePerformance GetCPUMonitor()
        {
            return new CPUHardwarePerformance(
                new HardwarePerformanceParams
                (
                    CategoryOptions.PROCESSOR_INFORMATION,
                    CounterOptions.PERCENT_PROCESSOR_UTILITY,
                    InstanceOptions.TOTAL_INSTANCE,
                    CategoryOptions.THIS_MACHINE,
                    CategoryOptions.READ_ONLY_ON
                )
            );
        }

        protected GPUHardwarePerformance GetGPUMonitor()
        {
            return new GPUHardwarePerformance(
                new HardwarePerformanceParams(
                    CategoryOptions.GPU_ENGINE,
                    CounterOptions.UTILIZATION_PERCENTAGE,
                    CategoryOptions.None,
                    CategoryOptions.THIS_MACHINE,
                    CategoryOptions.READ_ONLY_ON
                )
            );
        }
        
        protected MemoryHardwarePerformance GetMemoryMonitor()
        { 
            return new MemoryHardwarePerformance(
                new HardwarePerformanceParams(
                    CategoryOptions.MEMORY,
                    CounterOptions.AVAILABLE_MEGABYTES,
                    CategoryOptions.None,
                    CategoryOptions.THIS_MACHINE,
                    CategoryOptions.READ_ONLY_ON
                )
            );
        }
    }
}