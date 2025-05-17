
namespace DisplaySpace {
    public class Display
    {
        public string tag { get; set; } = "";
        public DisplayType type { get; set; }
        public Dimension dimension { get; set; }

        public Display(Dimension dimension)
        {
            this.dimension = dimension;
            this.SetTag();
        }

        protected void SetTag()
        {
            int width = this.dimension.width;
            int height = this.dimension.height;
            bool isSD = width == 640 && height == 480;
            bool isHD = width == 1280 && height == 720;
            bool isFullHD = width == 1920 && height == 1080;
            bool is2kQuadHD = width == 2560 && height == 1440;
            bool is4kUltraHD = width == 3480 && height == 2160;

            if (isSD)
            {
                this.tag = $"{height}p SD";
                this.type = DisplayType._SD;
            }

            else if (isHD)
            {
                this.tag = $"{height}p HD";
                this.type = DisplayType._HD;
            }

            else if (isFullHD)
            {
                this.tag = $"{height}p FUll HD";
                this.type = DisplayType._Full_HD;
            }

            else if (is2kQuadHD)
            {
                this.tag = $"{height}p 2k Quad HD";
                this.type = DisplayType._2K_Quad_HD;
            }

            else if (is4kUltraHD)
            {
                this.tag = $"{height}p 4k Ultra HD";
                this.type = DisplayType._4K_Ultra_HD;
            }
        }

        public override string ToString()
        {
            return $"{this.dimension.width} X {this.dimension.height}\t{this.tag}";
        }
    }
}