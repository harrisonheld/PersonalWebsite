using System.Drawing;

namespace PersonalWebsite.Filters
{
    public class Scale : IFilter
    {
        public string Name => "Scale";
        public string Description => "Resizes the image.";

        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; }

        public Image Filter(Image original)
        {
            Image result = new Bitmap(OutputWidth, OutputHeight);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(original, 0, 0, OutputWidth, OutputHeight);
            }
            return result;
        }
    }
}
