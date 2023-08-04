using System;
using System.Drawing;

namespace PersonalWebsite.Filters
{
    public class Dithering : IFilter
    {
        public string Name => "Floyd-Steinberg Dithering";
        public string Description => "Applies a dithering effect to the image.";

        public Image Filter(Image original)
        {
            Bitmap result = new Bitmap(original.Width, original.Height);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(original, 0, 0, original.Width, original.Height);

                // Apply Floyd-Steinberg dithering
                for (int y = 0; y < result.Height - 1; y++)
                {
                    for (int x = 1; x < result.Width - 1; x++)
                    {
                        Color oldColor = result.GetPixel(x, y);
                        Color newColor = GetClosestPaletteColor(oldColor);
                        result.SetPixel(x, y, newColor);

                        int quantErrorR = oldColor.R - newColor.R;
                        int quantErrorG = oldColor.G - newColor.G;
                        int quantErrorB = oldColor.B - newColor.B;

                        result.SetPixel(x + 1, y, Color.FromArgb(
                            Math.Min(255, result.GetPixel(x + 1, y).R + (quantErrorR * 7) / 16),
                            Math.Min(255, result.GetPixel(x + 1, y).G + (quantErrorG * 7) / 16),
                            Math.Min(255, result.GetPixel(x + 1, y).B + (quantErrorB * 7) / 16)
                        ));

                        result.SetPixel(x - 1, y + 1, Color.FromArgb(
                            Math.Min(255, result.GetPixel(x - 1, y + 1).R + (quantErrorR * 3) / 16),
                            Math.Min(255, result.GetPixel(x - 1, y + 1).G + (quantErrorG * 3) / 16),
                            Math.Min(255, result.GetPixel(x - 1, y + 1).B + (quantErrorB * 3) / 16)
                        ));

                        result.SetPixel(x, y + 1, Color.FromArgb(
                            Math.Min(255, result.GetPixel(x, y + 1).R + (quantErrorR * 5) / 16),
                            Math.Min(255, result.GetPixel(x, y + 1).G + (quantErrorG * 5) / 16),
                            Math.Min(255, result.GetPixel(x, y + 1).B + (quantErrorB * 5) / 16)
                        ));

                        result.SetPixel(x + 1, y + 1, Color.FromArgb(
                            Math.Min(255, result.GetPixel(x + 1, y + 1).R + (quantErrorR * 1) / 16),
                            Math.Min(255, result.GetPixel(x + 1, y + 1).G + (quantErrorG * 1) / 16),
                            Math.Min(255, result.GetPixel(x + 1, y + 1).B + (quantErrorB * 1) / 16)
                        ));
                    }
                }
            }
            return result;
        }

        private Color GetClosestPaletteColor(Color color)
        {
            int r = (int)Math.Round(color.R / 255.0) * 255;
            int g = (int)Math.Round(color.G / 255.0) * 255;
            int b = (int)Math.Round(color.B / 255.0) * 255;
            return Color.FromArgb(r, g, b);
        }
    }
}