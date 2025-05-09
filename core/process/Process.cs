using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO.Hashing;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace ProcessSpace {

    public class Process: System.Diagnostics.Process {
        protected UInt32 processId;
        protected System.Diagnostics.Process? process;
        public string name = "UNKNOWN";
        
        private string _hash;
        public string Hash
        {
            get => _hash;
        }

        [DllImport("psapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetModuleFileNameEx(
            SafeProcessHandle processHandle, 
            IntPtr moduleHandle, 
            StringBuilder baseName, 
            int size
        );

        public Process(Process process) {
            this.processId = process.processId;
            this.process = GetProcessById((int) processId);
            this.name = process.ProcessName;
            this._hash = GetHash();
        }

        public Process(UInt32 processId) {
            this.processId = processId;
            this.process = GetProcessById((int) processId);
            this.name = process.ProcessName;
            this._hash = GetHash();
        }

        public UInt32 GetID() {
            return this.processId;
        }

        public void kill(bool killChildren = false) {
            if (this.process is null) {
                throw new InvalidOperationException("Kill failed, process not instantiated.");
            }
            
            this.process.Kill(killChildren);
        }

        // Lol fix this!
        protected bool hasExited() {
            return true;
        }

        protected T GetValueOrDefault<T>(Func<T?> callback, T defaultValue) {
            try {
                return callback.Invoke() ?? defaultValue;
            } catch (Exception e) {
                return defaultValue;
            }
        }

        public MetaData GetMetaData() {
            return new MetaData() {
                id = (int) this.processId,
                name = this.name,
                hasExited = this.GetValueOrDefault<bool?>(() => this.process.HasExited, null),
                threadCount = this.GetValueOrDefault<int>(() => this.process.Threads.Count, -1),
                exitCode = this.GetValueOrDefault<int?>(() => this.process.ExitCode, null),
                exitTime = this.GetValueOrDefault<DateTime?>(() => this.process.ExitTime, null),
                // priority = Priority,
                processorTime = new ProcessTime() {
                    user = this.GetValueOrDefault<TimeSpan?>(
                        () => this.process.UserProcessorTime,
                        null
                    ),
                    privileged = this.GetValueOrDefault<TimeSpan?>(
                        () => this.process.PrivilegedProcessorTime,
                        null
                    ),
                    total = this.GetValueOrDefault<TimeSpan?>(
                        () => this.process.TotalProcessorTime,
                        null
                    )
                },
                memory = new ProcessMemory() {
                    pagedM = new MemoryUsage() {
                        peakSize = this.GetValueOrDefault<long?>(
                            () => this.process.PeakPagedMemorySize64,
                            null
                        ),
                        size = this.GetValueOrDefault<long?>(
                            () => this.process.PagedMemorySize64,
                            null
                        )
                    },
                    systemM = new MemoryUsage() {
                        peakSize = null,
                        size = this.GetValueOrDefault<long?>(
                            () => this.process.PagedSystemMemorySize64,
                            null
                        )
                    },
                    virtualM = new MemoryUsage() {
                        peakSize = this.GetValueOrDefault<long?>(
                            () => this.process.VirtualMemorySize64,
                            null
                        ),
                        size = this.GetValueOrDefault<long?>(
                            () => this.process.VirtualMemorySize64,
                            null
                        )
                    },
                    physicalM = new MemoryUsage() {
                        peakSize = this.GetValueOrDefault<long?>(
                            () => this.process.PeakWorkingSet64,
                            null
                        ),
                        size = this.GetValueOrDefault<long?>(
                            () => this.process.WorkingSet64,
                            null
                        )
                    }
                }
            };
        }

        protected string GetHash() {
            string metadataTag = this.GetMetaData().ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(metadataTag);
            byte[] xxHash3Value = XxHash3.Hash(bytes);

            return BitConverter.ToString(xxHash3Value);
        }
    }
}