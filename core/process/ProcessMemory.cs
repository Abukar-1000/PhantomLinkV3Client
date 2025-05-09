
namespace ProcessSpace {
    public class ProcessMemory {
        public MemoryUsage pagedM = new MemoryUsage(); 
        public MemoryUsage systemM = new MemoryUsage(); 
        public MemoryUsage virtualM = new MemoryUsage(); 
        public MemoryUsage physicalM = new MemoryUsage(); 
        public override string ToString() {
            return (
                $"{this.pagedM.ToString()}{this.systemM.ToString()}" +
                $"{this.virtualM.ToString()}{this.physicalM.ToString()}"
            );
        }        
    }

    public class MemoryUsage {
        public long? peakSize;
        public long? size;

        public override string ToString() {
            string value = "";
            if (peakSize is not null) {
                value += peakSize.ToString();
            }
            if (size is not null) {
                value += size.ToString();
            }
            return value;
        }
    }
}