

using SocketRoutes;

namespace ProcessSpace {

    public class ProcessWorkerParams {
        public bool running = true;
        public string route = Routes.proccess;
        public string controlRoute = Routes.control;

        public ProcessWorkerParams(
            bool running
        ) {
            this.running = running;
        }
    }
}