using System.Threading.Tasks;
using ProcessSpace;

namespace BackgrounderWorker {
    public class BackgroundWorkers {

        protected ProcessWorker processWorker;
        protected ProcessWorkerParams processWorkerParams;

        public BackgroundWorkers() {
            processWorkerParams = new ProcessWorkerParams(
                true
            );
            this.processWorker = new ProcessWorker(processWorkerParams);
        }
        public async Task StartProcessWorker() {
            await Task.Run(async () => await this.processWorker.StartProcessMonitor());
        }
    }
}