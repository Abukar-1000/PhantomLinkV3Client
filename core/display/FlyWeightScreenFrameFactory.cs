using System.Drawing.Imaging;
using System.Drawing;

namespace DisplaySpace
{
    public class FlyWeightScreenFrameFactory
    {
        protected string id = "";
        protected int length = 60;
        protected int pointer = 0;
        protected int width = 0;
        protected int height = 0;
        protected Bitmap bitmap = null;
        protected Graphics graphics = null;
        protected MemoryStream stream = new();
        protected List<ScreenFrame> frames = new();

        public FlyWeightScreenFrameFactory()
        {
            this.bitmap = new Bitmap(width, height);
            this.graphics = Graphics.FromImage(this.bitmap);
            this.InitializeFrames();
        }
        public FlyWeightScreenFrameFactory(
            int width,
            int height,
            int length,
            string id
        )
        {
            this.id = id;
            this.width = width;
            this.height = height;
            this.length = length;
            this.InitializeFrames();
            this.bitmap = new Bitmap(width, height);
            this.graphics = Graphics.FromImage(this.bitmap);
        }

        protected void InitializeFrames()
        {
            for (int i = 0; i < this.length; ++i)
            {
                this.frames.Add(
                    new ScreenFrame(
                        String.Empty,
                        this.width,
                        this.height,
                        this.id
                    )
                );
            }
        }
        public ScreenFrame GetFrame()
        {
            this.graphics.CopyFromScreen(0, 0, 0, 0, this.bitmap.Size);
            this.bitmap.Save(this.stream, ImageFormat.Jpeg);
            this.frames[this.pointer].image = Convert.ToBase64String(this.stream.ToArray());
            return this.frames[this.pointer];
        }

        public void Next()
        {
            this.pointer++;

            if (this.pointer >= this.length)
            {
                this.pointer = 0;
            }
            this.stream = new MemoryStream();
        }
    }
}