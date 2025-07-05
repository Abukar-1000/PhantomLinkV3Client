

using BackgrounderWorker;
using DeviceSpace;
using Microsoft.AspNetCore.SignalR.Client;
using Register.Models;

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
            const int FRAME_BUFFER_SIZE = 144;
            RegisterFrame registerFrame = _device.GetData();
            string id = registerFrame.id;
            Dimension deviceDimensions = registerFrame.display.dimension;
            var _connection = await ConnectToHub();
            bool isRegistered = await IsRegistered();

            var frameFactory = new FlyWeightScreenFrameFactory(
                deviceDimensions.width,
                deviceDimensions.height,
                FRAME_BUFFER_SIZE,
                id
            );

            while (true)
            {
                if (_params.running)
                {
                    ScreenFrame frame = frameFactory.GetFrame();
                    await _connection?.InvokeAsync("BrodcastFrame", frame);
                    frameFactory.Next();

                    if (_params.running is false)
                    {
                        await Task.Delay(10);
                    }
                }
            }
        }
    }
}