


namespace DisplaySpace
{
    public class ScreenFrame : IScreenFrame
    {
        public string image { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string id { get; set; }

        public ScreenFrame() { }
        public ScreenFrame(
            string image,
            int width,
            int height,
            string id
        )
        {
            this.image = image;
            this.width = width;
            this.height = height;
            this.id = id;
        }

        public override string ToString()
        {
            return $"{image?.Length} \t{width}x{height}";
        }
    }
}