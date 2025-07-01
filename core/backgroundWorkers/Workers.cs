using System.Threading.Tasks;
using DeviceSpace;
using DisplaySpace;
using HardwareSpace;
using ProcessSpace;

using HardwareBackground = HardwareSpace.BackgrounderWorker;

namespace BackgrounderWorker
{
    public class BackgroundWorkers
    {

        protected ProcessWorker processWorker;
        protected ScreenBroadcasterWorker screenBroadcasterWorker;
        protected HardwarePerformanceWorker hardwarePerformanceWorker;

        protected ProcessWorkerParams processWorkerParams;
        protected ScreenBroadcasterWorkerParams screenBroadcasterWorkerParams;
        protected HardwareBackground.HardwarePerformanceParams hardwarePerformanceParams;
        protected Device device;

        public BackgroundWorkers()
        {
            device = new Device();
            processWorkerParams = new ProcessWorkerParams(true);
            screenBroadcasterWorkerParams = new ScreenBroadcasterWorkerParams(true);
            hardwarePerformanceParams = new HardwareBackground.HardwarePerformanceParams(true);

            this.processWorker = new ProcessWorker(processWorkerParams, device);
            this.screenBroadcasterWorker = new ScreenBroadcasterWorker(screenBroadcasterWorkerParams, device);
            this.hardwarePerformanceWorker = new HardwarePerformanceWorker(hardwarePerformanceParams, device);
        }

        public async Task StartProcessWorker()
        {
            await this.HandleUnexpectedError(
                async () => await Task.Run(async () => await this.processWorker.StartProcessMonitor()),
                this.processWorkerParams
            );
        }

        public async Task StartScreenMonitor()
        {
            await this.HandleUnexpectedError(
                async () => await Task.Run(async () => await this.screenBroadcasterWorker.StartScreenMonitor()),
                this.screenBroadcasterWorkerParams
            );
        }

        public async Task StartHardwarePerformanceMonitor()
        {
            await this.HandleUnexpectedError(
                async () => await Task.Run(async () => await this.hardwarePerformanceWorker.StartPerformanceMonitor()),
                this.hardwarePerformanceParams
            );
        }

        protected async Task HandleUnexpectedError(Func<Task> worker, IBackgroundWorkerParams _params)
        {
            while (true)
            {
                try
                {
                    await worker();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n\n[{_params.route}] failed with exception \n{ex}");
                }
            }
        }
    }
}