using System.Drawing;
using System.Drawing.Imaging;

namespace PersonalWebsite.Models.Filters
{
    public class Tint : IFilter
    {
        public string Name => "Tint";
        public string Description => "Tint the image.";

        private Color _tint;
        private float _intensity;

        public Tint(Color tint, float intensity)
        {
            _tint = tint;
            _intensity = intensity;
        }

        public Bitmap Apply(Bitmap original)
        {
            Bitmap tintedImage = new Bitmap(original.Width, original.Height);

            using (Graphics graphics = Graphics.FromImage(tintedImage))
            {
                graphics.DrawImage(original, 0, 0, original.Width, original.Height);

                using (ImageAttributes attributes = new ImageAttributes())
                {
                    ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                        new float[] { _tint.R / 255.0f * _intensity + (1 - _intensity), 0, 0, 0, 0 },
                        new float[] { 0, _tint.G / 255.0f * _intensity + (1 - _intensity), 0, 0, 0 },
                        new float[] { 0, 0, _tint.B / 255.0f * _intensity + (1 - _intensity), 0, 0 },
                        new float[] { 0, 0, 0, 1, 0 },
                        new float[] { 0, 0, 0, 0, 1 }
                    });

                    attributes.SetColorMatrix(colorMatrix);

                    graphics.DrawImage(
                        original,
                        new Rectangle(0, 0, original.Width, original.Height),
                        0, 0, original.Width, original.Height,
                        GraphicsUnit.Pixel,
                        attributes
                    );
                }
            }

            return tintedImage;
        }
    }
}