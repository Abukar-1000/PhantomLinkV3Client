

using SocketRoutes;

namespace ProcessSpace {

    public class ProcessWorkerParams {
        public bool running = true;
        public string route = Routes.proccess;

        public ProcessWorkerParams(
            bool running
        ) {
            this.running = running;
        }
    }
}