using System.Threading.Channels;

namespace ProcessSpace {
    public class ProcessMonitor {
        private readonly ProcessPool _pool;
        private readonly ProcessPool _referencePool;

        // Implement in worker
        private readonly Channel<ProcessGroup> _channel;
        private readonly bool _running;
        private readonly Int32 _delay;

        public ProcessPool Pool {
            get => _pool;
        }
        public ProcessPool ReferencePool {
            get => _referencePool;
        }

        public ProcessMonitor() {
            _delay = 1000;
            _running = true;
            _pool = new ProcessPool();
            _referencePool = new ProcessPool();
            _channel = Channel.CreateUnbounded<ProcessGroup>();
        }
        public ProcessMonitor(UInt32 ProcessCount) {
            _delay = 1000;
            _running = true;
            _pool = new(ProcessCount);
            _referencePool = new(ProcessCount);
            _channel = Channel.CreateUnbounded<ProcessGroup>();
        }
    }
}