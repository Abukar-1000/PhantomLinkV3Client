using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ProcessSpace {

    public enum ProcessStatus {
        FailedInitialization = -2,
        Dead = -1,
        Alive = 1
    };

    public class ProcessPool {

        private UInt32 count = 0;
        private Dictionary<string, ProcessGroup> processMap = new Dictionary<string, ProcessGroup>();
        protected int pageSize = 20;
        public ProcessPool() {
            this.count = 512;
            this.Init();
        }

        public ProcessPool(UInt32 count) {
            if (count < 0) {
                throw new ArgumentOutOfRangeException("Negative process count provided");
            }

            this.count = count;
            this.Init();
        }


        protected ExecutionStatus MapProcess(Process p) {
            try {
                if (this.processMap.ContainsKey(p.name)) {
                    this.processMap[p.name]._group.Add(p);
                } else {
                    this.processMap[p.name] = new (p);
                }
                return ExecutionStatus.Success;
            } catch (Exception e) {
                return ExecutionStatus.Failed;
            }
        }

        protected ExecutionStatus Init() {
            List<Process> proclist = new List<Process>();
            UInt32 arrayBytesSize = this.count * sizeof(UInt32);
            UInt32[] processIds = new UInt32[this.count];
            UInt32 bytesCopied;

            bool success = EnumProcesses(processIds, arrayBytesSize, out bytesCopied);
            if (success is false) {
                return ExecutionStatus.Failed;
            }

            UInt32 numIdsCopied = bytesCopied >> 2;
            for (UInt32 index = 0; index < numIdsCopied; index++)
            {
                Process process = new Process(processIds[index]);
                this.MapProcess(process);
            }

            return ExecutionStatus.Success;
        }

        public ProcessPool(string name) {}

        public void ViewAllHash() {
            foreach (var pair in this.processMap) {
                Console.WriteLine($"Name:{pair.Key}\tHash\t{pair.Value.GetHash()}");
            }
        }

        // Add update group method after outdated values detected
        
        public List<Process> ListAll() {
            
            this.Init();
            List<Process> proclist = new List<Process>();
            this.processMap.Keys.ToList().ForEach(k => {
                this.processMap[k]._group.ForEach(process => proclist.Add(process));
            });

            for (int i = 0; i < proclist.Count; ++i) {
                Console.WriteLine($"[{i}]\t process: {proclist[i].name}\t ID: {proclist[i].GetMetaData().ToString()}");
            }
            return proclist;
        }

        public string ViewAllProcessJson(int? page = 0) {
            this.Init();
            List<MetaData> allData = new();

            this.processMap.Keys.ToList().ForEach(processName => {
                List<Process> processes = this.processMap[processName]._group;
                processes.ForEach(process => {
                    MetaData meta = process.GetMetaData();
                    allData.Add(meta);
                });
            });

            if (page is not null) {
                int start = (int) ((pageSize * page) < allData.Count? (pageSize * page): -1);
                
                if (start >= 0) {
                    allData = allData.Slice(start, this.pageSize);
                }
            }

            string jsonPayload = JsonConvert.SerializeObject(allData, Formatting.Indented);
            return jsonPayload;
        }

        [DllImport("psapi")]
        private static extern bool EnumProcesses(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] [In][Out] UInt32[] processIds,
            UInt32 arraySizeBytes,
            [MarshalAs(UnmanagedType.U4)] out UInt32 bytesCopied
        );
    }
}