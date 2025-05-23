


using Register.Models;

namespace DeviceSpace
{
    public interface IDevice
    {
        public void displayInfo();
        public RegisterFrame GetData();
    }
}