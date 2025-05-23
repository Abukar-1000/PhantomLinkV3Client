using System.Runtime.InteropServices;
using System;
using DisplaySpace;
using Register.Models;

namespace DeviceSpace {

    public class Device: IDevice
    {

        protected string name = Environment.MachineName;
        protected string version = Environment.Version.ToString();
        protected string username = Environment.UserName;
        protected Display display;

        protected int SM_CXSCREEN = 0;
        protected int SM_CYSCREEN = 1;

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        public string id;

        public Device()
        {
            this.id = this.name + this.username + this.version;
            this.display = new Display(
                new Dimension(
                    GetSystemMetrics(this.SM_CXSCREEN),
                    GetSystemMetrics(this.SM_CYSCREEN)
                )
            );
        }

        public void displayInfo()
        {
            Console.WriteLine($"\t {this.name} \t");
            Console.WriteLine($"\t {this.version} \t");
            Console.WriteLine($"\t {this.username} \t");
            Console.WriteLine($"\t {this.display.ToString()} \t");
        }

        public RegisterFrame GetData()
        {
            return new RegisterFrame(
                this.name,
                this.version,
                this.username,
                this.display,
                this.id
            );
        }
        
    }
}