using System.ComponentModel;
using System.IO.Hashing;
using System.Text;

namespace ProcessSpace {

    public class ProcessGroup {
        public List<Process> _group = new();
        private string _hash;
        public string Hash
        {
            get => _hash;
        }

        public ProcessGroup() {
            this._hash = this.GetHash();
        }
        public ProcessGroup(Process p) {
            this._group.Add(p);
            this._hash = this.GetHash();
        }

        public ProcessGroup(List<Process> l) {
            this._group = new(l.Count);
            foreach (Process p in l) {
                this._group.Add(new Process(p));
            }

            this._hash = this.GetHash();
        }

        public ProcessGroup(IEnumerable<Process> l) {
            this._group = new(l.Count());
            foreach (Process p in l) {
                this._group.Add(new ProcessSpace.Process(p));
            }
            
            this._hash = this.GetHash();
        }

        public string GetHash() {
            string groupHash =  "";
            this._group.ForEach((Process p) => {
                groupHash += p.Hash;
            });
            return groupHash;
        }

    }
}