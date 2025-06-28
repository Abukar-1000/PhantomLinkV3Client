



using System.Diagnostics;

namespace HardwareSpace
{
    public abstract class BaseHardwarePerformance
    {
        protected IHardwarePerformanceParams PerformanceParams { get; set; }
        protected PerformanceCounter? performanceCounter { get; set; } = null;
        public BaseHardwarePerformance()
        {
            this.PerformanceParams = new HardwarePerformanceParams();
            this.performanceCounter = new PerformanceCounter();
        }
        public BaseHardwarePerformance(IHardwarePerformanceParams _params)
        {
            this.PerformanceParams = _params;

            if (_params.Category != "GPU Engine")
            {
                this.performanceCounter = new PerformanceCounter()
                {
                    CategoryName = _params.Category,
                    CounterName = _params.Counter,
                    InstanceName = _params.Instance,
                    MachineName = _params.Machine ?? ".",
                    ReadOnly = _params.ReadOnly ?? true
                };
            }
        }

        public override string ToString()
        {
            return $"{this.PerformanceParams.Category} \t {this.PerformanceParams.Counter} \t {this.PerformanceParams.Instance} \t {this.PerformanceParams.Machine}";
        }
    }
}