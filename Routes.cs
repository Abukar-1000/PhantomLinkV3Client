
using System.ComponentModel;

namespace SocketRoutes {

    class BaseRoutes {
        [Description("Base Route")]
        public static string Server = "http://10.0.0.178:80";
    };

    public class Routes {

        [Description("Server Control Route")]
        public static string control = BaseRoutes.Server + "/socket";

        [Description("Server Process Management Route")]
        public static string proccess = BaseRoutes.Server + "/process";

        [Description("Server Screen Monitoring Route")]
        public static string screenBroadcasterWorker = BaseRoutes.Server + "/screenBrodcast";
    }
}