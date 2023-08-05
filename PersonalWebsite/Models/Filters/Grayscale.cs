using System.Drawing;
using System.Drawing.Imaging;

namespace PersonalWebsite.Models.Filters
{
    public class Grayscale : IFilter
    {
        public string Name => "Grayscale";
        public string Description => "Makes the image black and white.";

        public Grayscale()
        {
        }

        public Image Apply(Image original)
        {
            Bitmap grayscaleImage = new Bitmap(original.Width, original.Height);

            using (Graphics graphics = Graphics.FromImage(grayscaleImage))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                    new float[][]
                    {
                        new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                        new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                        new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                        new float[] { 0, 0, 0, 1, 0 },
                        new float[] { 0, 0, 0, 0, 1 }
                    });

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                graphics.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                                   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }

            return grayscaleImage;
        }
    }
}
