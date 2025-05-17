using DisplaySpace;


namespace Register.Models
{
    public class RegisterFrame
    {
        public string name { get; set; }
        public string version { get; set; }
        public string username { get; set; }
        public Display display { get; set; }

        public string id { get; set; }
        
        public RegisterFrame(
            string name,
            string version,
            string username,
            Display display,
            string id
        )
        {
            this.name = name;
            this.version = version;
            this.username = username;
            this.display = display;
            this.id = id;
        }
    }
}