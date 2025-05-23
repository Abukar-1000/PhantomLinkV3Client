

using BackgrounderWorker;
using DeviceSpace;
using Microsoft.AspNetCore.SignalR.Client;
using Register.Models;
using System.Drawing;
using System.Drawing.Imaging;

namespace DisplaySpace
{
    public class ScreenBroadcasterWorker : BackgroundWorkerBase
    {

        protected IBackgroundWorkerParams _params;
        protected IDevice _device;

        public ScreenBroadcasterWorker() { }
        public ScreenBroadcasterWorker(
            ScreenBroadcasterWorkerParams _params,
            Device device
        )
            : base(_params, device)
        {
            this._params = _params;
            this._device = device;
        }

        public async Task StartScreenMonitor()
        {
            RegisterFrame registerFrame = _device.GetData();
            string id = registerFrame.id;
            Dimension deviceDimensions = registerFrame.display.dimension;
            Console.WriteLine($"Screen thread!!! \t{_params.running}");
            var _connection = await ConnectToHub();
            bool isRegistered = await IsRegistered();

            while (true)
            {
                if (_params.running)
                {
                    using (var bitmap = new Bitmap(deviceDimensions.width, deviceDimensions.height))
                    {
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);    
                            using (var stream = new MemoryStream())
                            {
                                bitmap.Save(stream, ImageFormat.Jpeg);
                                ScreenFrame frame = new ScreenFrame(
                                    Convert.ToBase64String(stream.ToArray()),
                                    deviceDimensions.width,
                                    deviceDimensions.height,
                                    id
                                );

                                await _connection?.InvokeAsync("BrodcastFrame", frame);
                            }
                        }
                    }

                    if (_params.running is false)
                    { 
                        await Task.Delay(1);
                    }
                }
            }
        }
    }
}