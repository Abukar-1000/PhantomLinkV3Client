

using BackgrounderWorker;
using SocketRoutes;

namespace ProcessSpace {

    public class ProcessWorkerParams: IBackgroundWorkerParams {
        public bool running { get; set; } = true;
        public string route { get; set; } = Routes.proccess;
        public string controlRoute { get; set; } = Routes.control;

        public ProcessWorkerParams() {}
        public ProcessWorkerParams(
            bool running
        ) {
            this.running = running;
        }
    }
}