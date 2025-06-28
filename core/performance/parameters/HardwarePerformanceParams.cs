

namespace HardwareSpace
{
    public class HardwarePerformanceParams : IHardwarePerformanceParams
    {
        public string? Category { get; private set; }
        public string? Counter { get; private set; }
        public string? Instance { get; private set; }
        public string? Machine { get; private set; }
        public bool? ReadOnly { get; private set; }
        public HardwarePerformanceParams() { }
        public HardwarePerformanceParams(
            string category,
            string counter,
            string instance
        )
        {
            this.Category = category;
            this.Counter = counter;
            this.Instance = instance;
        }
        public HardwarePerformanceParams(
            string category,
            string counter,
            string instance,
            string machine
        )
        {
            this.Category = category;
            this.Counter = counter;
            this.Instance = instance;
            this.Machine = machine;
        }
        public HardwarePerformanceParams(
            string category,
            string counter,
            string instance,
            string machine,
            bool readOnly
        )
        {
            this.Category = category;
            this.Counter = counter;
            this.Instance = instance;
            this.Machine = machine;
            this.ReadOnly = readOnly;
        }
        
    }
}