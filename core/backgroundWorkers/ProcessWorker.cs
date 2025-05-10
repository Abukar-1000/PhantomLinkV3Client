using ProcessSpace;

namespace BackgrounderWorker {
    public class ProcessWorker {

        protected List<ProcessSnapshot>? CheckForDeadProcesses(ProcessPool monitor) {
            List<ProcessSnapshot> deadChanges = new();
            
            foreach (var pair in monitor.GetCurrentShot()) {
                string processGroup = pair.Key;
                string currentGroupHash = pair.Value.Hash;
                var group = pair.Value._group;

                bool isActive = group.All(process => process.IsRunning == false);
                if (isActive) {
                    deadChanges.Add(new ProcessSnapshot(
                        processGroup,
                        ProcessStatus.Dead,
                        pair.Value
                    ));
                }
            }

            if (deadChanges.Count == 0) {
                return null;
            }

            return deadChanges;
        }

        protected List<ProcessSnapshot>? CheckForNewProcesses(ProcessPool monitor) {
            List<ProcessSnapshot> newChanges = new();

            foreach (var pair in monitor.GetCurrentShot()) {
                string processGroup = pair.Key;
                string currentGroupHash = pair.Value.Hash;
                var group = pair.Value._group;

                bool isActive = group.Any(process => process.IsRunning);
                if (isActive) {
                    newChanges.Add(new ProcessSnapshot(
                        processGroup,
                        ProcessStatus.Alive,
                        pair.Value
                    ));
                }
            }
            
            if (newChanges.Count == 0) {
                return null;
            }

            return newChanges;
        }

        // remove
        public static SnapshotStatus GetSnapshotStatus(
            string processName,
            Dictionary<string, ProcessGroup> old,
            Dictionary<string, ProcessGroup> current
        ) {
            bool isInOld = old.ContainsKey(processName);
            bool isInCurrent = current.ContainsKey(processName);
            
            if (isInOld && isInCurrent) {
                return SnapshotStatus.InBoth;
            }

            if (isInOld && !isInCurrent) {
                return SnapshotStatus.InOld;
            }

            if (isInCurrent && !isInOld) {
                return SnapshotStatus.InCurrent;
            }

            return SnapshotStatus.Undecided;
        }

        public async Task StartProcessMonitor(bool running) {
            const int delay = 10;
            ProcessPool monitor = new();

            while (running) {
                
                Console.WriteLine($"\n\n\n\n\n\n\n\n\n");
                monitor.Update();
                List<ProcessSnapshot>? newProcesses = this.CheckForNewProcesses(monitor);
                List<ProcessSnapshot>? deadProcesses = this.CheckForDeadProcesses(monitor);

                if (newProcesses is not null) {
                    foreach (ProcessSnapshot processSnapshot in newProcesses) {
                        Console.WriteLine($"[+] \tName:\t{processSnapshot.processName} Active");
                    }
                }

                if (deadProcesses is not null) {
                    foreach (ProcessSnapshot processSnapshot in deadProcesses) {
                        Console.WriteLine($"[-] \tName:\t{processSnapshot.processName} Inactive");
                    }
                }
                
                await Task.Delay(delay);
            }
        }
    }
}