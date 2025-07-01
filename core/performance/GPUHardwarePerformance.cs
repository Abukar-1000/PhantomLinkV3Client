
using System.Diagnostics;

namespace HardwareSpace
{
    public class GPUHardwarePerformance : BaseHardwarePerformance, IHardwarePerformance
    {
        public float value { get; set; }
        public GPUHardwarePerformance() { }

        public GPUHardwarePerformance(IHardwarePerformanceParams _params)
        : base(_params)
        { }

        public float Next()
        {
            var availableGPUCounters = this.GetAvailableGPUCounters();
            if (availableGPUCounters.Count == 0)
            {
                this.value = 0;
                return 0;
            }

            availableGPUCounters.ForEach(GPUCounter => GPUCounter.NextValue());
            this.value = availableGPUCounters.Sum(GPUCounter => GPUCounter.NextValue());
            this.value = (float) Math.Round( this.value );
            return this.value;
        }

        protected List<PerformanceCounter> GetAvailableGPUCounters()
        {
            var gpuCategory = new PerformanceCounterCategory("GPU Engine");
            var counters = gpuCategory.GetInstanceNames();

            var availableCounters = counters
                                        .SelectMany(counterName => gpuCategory.GetCounters(counterName))
                                        .Where(counter => counter.CounterName.Equals(this.PerformanceParams.Counter))
                                        .ToList();

            return availableCounters;
        }
    }
}