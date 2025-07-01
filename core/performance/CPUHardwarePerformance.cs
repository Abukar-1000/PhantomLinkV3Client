

namespace HardwareSpace
{
    public class CPUHardwarePerformance : BaseHardwarePerformance, IHardwarePerformance
    {
        public float value { get; set; }
        public CPUHardwarePerformance() { }

        public CPUHardwarePerformance(IHardwarePerformanceParams _params)
        : base(_params)
        { }

        public float Next()
        {
            if (this.performanceCounter is null)
            {
                throw new InvalidOperationException("CPU Hardware Performance instance needs a reference to a performance counter to track performance. Currntly a null reference.");
            }

            this.value = this.performanceCounter.NextValue();
            this.value = (float) Math.Round( this.value );
            return this.value;
        }
    }
}