


namespace BackgrounderWorker
{
    public interface IBackgroundWorkerParams
    { 
        public bool running { get; set; }
        public string route { get; set; }
        public string controlRoute { get; set; }
    }
}