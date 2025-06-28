using BackgrounderWorker;
using DeviceSpace;
using HardwareSpace.HardwarePerformance.Options;
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

            var cpuCounter = new CPUHardwarePerformance(
                new HardwarePerformanceParams
                ( 
                    CategoryOptions.PROCESSOR_INFORMATION,
                    CounterOptions.PERCENT_PROCESSOR_UTILITY,
                    InstanceOptions.TOTAL_INSTANCE,
                    CategoryOptions.THIS_MACHINE,
                    CategoryOptions.READ_ONLY_ON
                )
            );

            var ramCounter = new MemoryHardwarePerformance(
                new HardwarePerformanceParams(
                    CategoryOptions.MEMORY,
                    CounterOptions.AVAILABLE_MEGABYTES,
                    CategoryOptions.None,
                    CategoryOptions.THIS_MACHINE,
                    CategoryOptions.READ_ONLY_ON
                )
            );
            
            var gpuCounter = new GPUHardwarePerformance(
                new HardwarePerformanceParams(
                    CategoryOptions.GPU_ENGINE,
                    CounterOptions.UTILIZATION_PERCENTAGE,
                    CategoryOptions.None,
                    CategoryOptions.THIS_MACHINE,
                    CategoryOptions.READ_ONLY_ON
                )
            );

            while (true)
            {
                if (_params.running)
                {
                    Console.WriteLine($"[Performance Monitor]. \tCPU: {(int) cpuCounter.Next()} \tMEM: {(int) ramCounter.Next()} \tGPU: {(int) gpuCounter.Next()}");
                    await Task.Delay(500);
                }

                if (_params.running is false)
                {
                    await Task.Delay(10);
                }
            }
        }
    }
}