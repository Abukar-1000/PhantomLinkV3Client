using ProcessSpace;

namespace BackgrounderWorker {
    public static class ProcessWorker {

        // remove
        public static List<ProcessSnapshot>? CheckForDeadProcesses(
            ProcessPool oldPool,
            ProcessPool currentPool
        ) {
            List<ProcessSnapshot> deadChanges = new();
            
            var current = currentPool.GetCurrentShot();
            var old = oldPool.GetCurrentShot();

            foreach (var pair in current) {
                string processGroup = pair.Key;
                string currentGroupHash = pair.Value.Hash;
                
                SnapshotStatus status = ProcessWorker.GetSnapshotStatus(
                    processGroup,
                    old,
                    current
                );

                if (status == SnapshotStatus.InOld) {
                    deadChanges.Add(new ProcessSnapshot(
                        processGroup,
                        ProcessStatus.Dead
                    ));
                }
            }

            if (deadChanges.Count == 0) {
                return null;
            }

            return deadChanges;
        }

        // remove
        public static List<ProcessSnapshot>? CheckForNewProcesses(
            ProcessPool oldPool,
            ProcessPool currentPool
        ) {
            List<ProcessSnapshot> newChanges = new();

            var current = currentPool.GetCurrentShot();
            var old = oldPool.GetCurrentShot();

            foreach (var pair in current) {
                string processGroup = pair.Key;
                string currentGroupHash = pair.Value.Hash;
                
                SnapshotStatus status = ProcessWorker.GetSnapshotStatus(
                    processGroup,
                    old,
                    current
                );

                string oldHash = oldPool.GetProcessGroupHash(processGroup) ?? "";
                string currentHash = currentPool.GetProcessGroupHash(processGroup) ?? "";
                bool isOutdatedHash = currentHash != oldHash;

                if (status == SnapshotStatus.InBoth) {
                    continue;
                }

                if (status == SnapshotStatus.InCurrent) {
                    newChanges.Add(new ProcessSnapshot(
                        processGroup,
                        ProcessStatus.Alive
                    ));
                }
                else if (isOutdatedHash && status != SnapshotStatus.InOld) {
                    newChanges.Add(new ProcessSnapshot(
                        processGroup,
                        ProcessStatus.Alive
                    ));
                }
            }

            if (newChanges.Count == 0) {
                return null;
            }

            return newChanges;
        }

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

        public static async Task StartProcessMonitor(bool running) {
            const int delay = 1000 * 10;
            ProcessPool monitor = new();

            while (running) {
                
                Console.WriteLine($"\n\n\n\n\n\n\n\n\n");
                monitor.Update();
                foreach (var pair in monitor.GetCurrentShot()) {
                    string processGroup = pair.Key;
                    string currentGroupHash = pair.Value.Hash;
                    var group = pair.Value._group;

                    bool isActive = group.Any(process => process.IsRunning);
                    if (isActive) {
                        Console.WriteLine($"[+] \tName:\t{processGroup} Active");
                    }
                    else {
                        Console.WriteLine($"[+] \tName:\t{processGroup} Inactive");
                    }
                }
                await Task.Delay(delay);
            }
        }
    }
}