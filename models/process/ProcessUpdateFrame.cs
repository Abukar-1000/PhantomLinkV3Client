

namespace ProcessSpace.Models {

    public class ProcessUpdateFrame {
        public string deviceID { get; set; }
        public string processName { get; set; }
        public ProcessStatus status { get; set; }
        public List<UInt32> processIds { get; set; }
        public DateTime timestamp { get; set; }
        public ProcessUpdateFrame() {
            this.deviceID = "0";
            this.processName = "UNKNOWN";
            this.status = ProcessStatus.Dead;
            this.timestamp = DateTime.Now;
            this.processIds = new();
        }
        public ProcessUpdateFrame(ProcessSnapshot snapshot) {
            this.deviceID = snapshot.deviceID;
            this.processName = snapshot.processName;
            this.status = snapshot.status;
            this.timestamp = DateTime.Now;
            this.processIds = snapshot.group._group.Select(process => process.ID).ToList();
        }
        public override string ToString() {
            return $"ID:  {deviceID}\tProcess:  {processName}\tStatus:  {status}\tIDs:  {processIds.ToString()}\tTime:  {timestamp.ToString()}";
        }
    }
}