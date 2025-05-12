
using System.ComponentModel;

namespace SocketRoutes {

    class BaseRoutes {
        [Description("Base Route")]
        public static string Server = "http://10.0.0.178:80";
    };

    public class Routes {

        [Description("Server Control Route")]
        public static string control = BaseRoutes.Server + "/socket";
        public static string proccess = BaseRoutes.Server + "/process";
    }
}