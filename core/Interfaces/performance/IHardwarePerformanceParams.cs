

namespace HardwareSpace
{
    public interface IHardwarePerformanceParams
    { 
        public string? Category { get; }
        public string? Counter { get; }
        public string? Instance { get; }
        public string? Machine { get; }
        public bool? ReadOnly { get; }
    }
}