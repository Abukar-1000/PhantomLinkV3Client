using System.Management;


namespace HardwareSpace
{
    public class MemoryHardwarePerformance : BaseHardwarePerformance, IHardwarePerformance
    {
        public float value { get; set; }
        protected double totalMemoryAvailable { get; set; }
        public MemoryHardwarePerformance() { }

        public MemoryHardwarePerformance(IHardwarePerformanceParams _params)
        : base(_params)
        { 
            this.totalMemoryAvailable = this.QueryAvailableMemory();
        }

        public float Next()
        {
            if (this.performanceCounter is null)
            {
                throw new InvalidOperationException("Memory Hardware Performance instance needs a reference to a performance counter to track performance. Currntly a null reference.");
            }

            float nextValue = this.performanceCounter.NextValue();
            double usedPercentage = ( this.totalMemoryAvailable - nextValue ) / this.totalMemoryAvailable;
            this.value = (float) usedPercentage * 100;

            return this.value;
        }

        protected double QueryAvailableMemory()
        {
            double availableMemory = 0;

            ulong totalMemoryBytes = 0;
            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_PhysicalMemory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                totalMemoryBytes += (ulong)Convert.ToUInt64(result["Capacity"]);
            }

            // Convert total memory to MB
            const double ONE_MB = (1024.0 * 1024.0);
            availableMemory = (double)totalMemoryBytes / ONE_MB;

            return availableMemory;
        }
    }
}