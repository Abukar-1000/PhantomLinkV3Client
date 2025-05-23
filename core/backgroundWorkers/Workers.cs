using System.Threading.Tasks;
using DeviceSpace;
using DisplaySpace;
using ProcessSpace;

namespace BackgrounderWorker {
    public class BackgroundWorkers
    {

        protected ProcessWorker processWorker;
        protected ScreenBroadcasterWorker screenBroadcasterWorker;

        protected ProcessWorkerParams processWorkerParams;
        protected ScreenBroadcasterWorkerParams screenBroadcasterWorkerParams;
        protected Device device;

        public BackgroundWorkers()
        {
            device = new Device();
            processWorkerParams = new ProcessWorkerParams(
                true
            );
            screenBroadcasterWorkerParams = new ScreenBroadcasterWorkerParams(
                true
            );

            this.processWorker = new ProcessWorker(processWorkerParams, device);
            this.screenBroadcasterWorker = new ScreenBroadcasterWorker(screenBroadcasterWorkerParams, device);
        }

        public async Task StartProcessWorker()
        {
            await Task.Run(async () => await this.processWorker.StartProcessMonitor());
        }
        
        public async Task StartScreenMonitor()
        {
            await Task.Run(async () => await this.screenBroadcasterWorker.StartScreenMonitor());
        }
    }
}