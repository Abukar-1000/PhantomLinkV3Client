
namespace ProcessSpace {
    public class ProcessTime {
        public TimeSpan? user;
        public TimeSpan? privileged;
        public TimeSpan? total;

        public override string ToString() {
            string value = "";

            if (user is not null) {
                value += user.Value.ToString();
            }
            if (privileged is not null) {
                value += privileged.ToString();
            }
            if (total is not null) {
                value += total.ToString();
            }
            return value;
        }
    }
}