using System.Drawing;

namespace PersonalWebsite.Models.Filters
{
    public class Scale : IFilter
    {
        public string Name => "Scale";
        public string Description => "Resizes the image.";

        private int _outputWidth { get; set; }
        private int _outputHeight { get; set; }

        public Scale(int outputWidth, int outputHeight)
        {
            _outputWidth = outputWidth;
            _outputHeight = outputHeight;
        }

        public Image Apply(Image original)
        {
            Image result = new Bitmap(_outputWidth, _outputHeight);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(original, 0, 0, _outputWidth, _outputHeight);
            }
            return result;
        }
    }
}
