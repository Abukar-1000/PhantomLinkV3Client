

using BackgrounderWorker;
using SocketRoutes;

namespace DisplaySpace
{
    public class ScreenBroadcasterWorkerParams: IBackgroundWorkerParams
    { 
        // Reset to false after testing and implementing switch
        public bool running { get; set; } = true;
        public string route { get; set; } = Routes.screenBroadcasterWorker;
        public string controlRoute { get; set; } = Routes.control;

        public ScreenBroadcasterWorkerParams() {}
        public ScreenBroadcasterWorkerParams(
            bool running
        ) {
            this.running = running;
        }
    }
}