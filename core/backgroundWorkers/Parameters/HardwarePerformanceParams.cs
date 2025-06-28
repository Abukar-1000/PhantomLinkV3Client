

using BackgrounderWorker;
using SocketRoutes;

namespace HardwareSpace.BackgrounderWorker
{
    public class HardwarePerformanceParams : IBackgroundWorkerParams
    { 
        public bool running { get; set; } = true;
        public string route { get; set; } = Routes.hardwarePerformanceWorker;
        public string controlRoute { get; set; } = Routes.control;

        public HardwarePerformanceParams() {}
        public HardwarePerformanceParams(
            bool running
        ) {
            this.running = running;
        }
    }
}