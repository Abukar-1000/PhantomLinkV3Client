
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace SocketRoutes
{
    public class AppContext
    { 
        public string baseRoute { get; set; }
        public bool isProd { get; set; }
    }

    class BaseRoutes
    {
        [Description("Base Route")]
        public static string Server = GetConfiguration().baseRoute;

        public static AppContext GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
            IConfiguration configuration = builder.Build();
            bool isProduction = configuration["environmentVariables:IS_PROD"] == "true" ? true : false;
            string baseRoute = isProduction ? configuration["environmentVariables:PROD:BASE_ADDRESS"] : configuration["environmentVariables:DEV:BASE_ADDRESS"];

            Console.WriteLine($"{isProduction}\t {baseRoute}");
            return new AppContext
            {
                isProd = isProduction,
                baseRoute = baseRoute
            };
        }
    };

    public class Routes
    {

        [Description("Server Control Route")]
        public static string control = BaseRoutes.Server + "/socket";

        [Description("Server Process Management Route")]
        public static string proccess = BaseRoutes.Server + "/process";

        [Description("Server Screen Monitoring Route")]
        public static string screenBroadcasterWorker = BaseRoutes.Server + "/screenBrodcast";

        [Description("CPU, Memory, Disk, and GPU Monitoring Route")]
        public static string hardwarePerformanceWorker = BaseRoutes.Server + "/hardware/performance";
    }
}