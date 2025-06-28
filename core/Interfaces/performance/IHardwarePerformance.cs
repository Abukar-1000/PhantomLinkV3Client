

namespace HardwareSpace
{
    public interface IHardwarePerformance
    {
        public float value { get; }
        public float Next();
    }
}