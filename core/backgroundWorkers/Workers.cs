using System.Threading.Tasks;

namespace BackgrounderWorker {
    public class BackgroundWorkers {

        protected ProcessWorker processWorker;

        public BackgroundWorkers() {
            this.processWorker = new ProcessWorker();
        }
        public async Task StartProcessWorker() {
            await Task.Run(async () => await this.processWorker.StartProcessMonitor(true));
        }
    }
}