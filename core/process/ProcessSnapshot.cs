

namespace ProcessSpace {
    public enum ProcessStatus {
        FailedInitialization = -2,
        Dead = -1,
        Alive = 1
    };

    public enum SnapshotStatus {
        InOld = -2,
        InCurrent = -1,
        InBoth = 1,
        Undecided
    };

    public class ProcessSnapshot {
        public string processName;
        public ProcessStatus status;
        public ProcessSnapshot() {}
        public ProcessSnapshot(string processName, ProcessStatus status) {
            this.processName = processName;
            this.status = status;
        }
    }
}