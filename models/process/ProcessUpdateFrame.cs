

namespace ProcessSpace.Models {

    public class ProcessUpdateFrame {
        public string deviceID { get; set; }
        public string processName { get; set; }
        public string status { get; set; }
        public List<UInt32> processIds { get; set; }
        public string timestamp { get; set; }
        public ProcessUpdateFrame() {
            this.deviceID = "0";
            this.processName = "UNKNOWN";
            this.status = ProcessStatus.Dead.ToString();
            this.timestamp = DateTime.Now.ToString("MM/dd/yy HH:mm tt");
            this.processIds = new();
        }
        public ProcessUpdateFrame(ProcessSnapshot snapshot) {
            this.deviceID = snapshot.deviceID;
            this.processName = snapshot.processName;
            this.status = snapshot.status.ToString();
            this.timestamp = DateTime.Now.ToString("MM/dd/yy HH:mm tt");
            this.processIds = snapshot.group._group.Select(process => process.ID).ToList();
        }
        public override string ToString() {
            return $"ID:  {deviceID}\tProcess:  {processName}\tStatus:  {status}\tIDs:  {processIds.ToString()}\tTime:  {timestamp.ToString()}";
        }
    }
}