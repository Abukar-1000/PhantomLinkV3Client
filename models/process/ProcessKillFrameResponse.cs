

namespace ProcessSpace.Models {
    public class ProcessKillFrameResponse: ProcessKillFrame {

        public int response { get; set; }
        
        public ProcessKillFrameResponse() {}
        public ProcessKillFrameResponse(
            ProcessKillFrame frame, 
            int status
        ) {
            this.processName = frame.processName;
            this.deviceId = frame.deviceId;
            this.response = status;
        }
    }
}