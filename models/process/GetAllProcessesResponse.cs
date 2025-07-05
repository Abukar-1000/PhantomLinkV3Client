
namespace ProcessSpace.Models {
    public class GetAllProcessesResponse
    {
        public string clientId { get; set; }
        public string deviceId { get; set; }
        public ProcessUpdateFrame process { get; set; }

        public GetAllProcessesResponse(
            string clientId,
            string deviceId,
            ProcessSnapshot frame
        )
        {
            this.clientId = clientId;
            this.deviceId = deviceId;
            this.process = new ProcessUpdateFrame(frame);            
        }
    }
}