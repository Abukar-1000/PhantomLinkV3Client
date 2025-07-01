

// HeartbeatFrame


namespace HardwarePerformance.Models
{
    public class HeartbeatFrame
    {
        public CPUData CPU { get; set; }
        public GPUData GPU { get; set; }
        public MemoryData Memory { get; set; }

        public string id { get; set; }

        public HeartbeatFrame()
        {
            this.CPU = new CPUData();
            this.GPU = new GPUData();
            this.Memory = new MemoryData();
        }
        
        public void Configure(
            float CPU,
            float GPU,
            float Memory
        )
        {
            this.CPU.value = CPU;
            this.GPU.value = GPU;
            this.Memory.value = Memory;
        }
    }
}