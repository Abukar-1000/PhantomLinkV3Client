
namespace HardwarePerformance.Models
{
    public abstract class BaseData
    {
        public float value { get; set; }
        public BaseData() { }
        public BaseData(float value)
        {
            this.value = value;
        }
    }
}