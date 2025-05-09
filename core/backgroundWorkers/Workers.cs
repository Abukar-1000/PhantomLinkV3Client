using System.Threading.Tasks;

namespace BackgrounderWorker {
    public static class BackgroundWorkers {

        public static async Task StartProcessWorker() {
            await Task.Run(async () => await ProcessWorker.StartProcessMonitor(true));
        }
    }
}