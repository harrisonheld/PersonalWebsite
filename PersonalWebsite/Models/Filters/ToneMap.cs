using System.Drawing;
using System.Drawing.Imaging;

namespace PersonalWebsite.Models.Filters
{
    public class ToneMap : IFilter
    {
        public string Name => "Tone Map";
        public string Description => "idk man";

        private Color[] _tones;

        public ToneMap(Color[] tones)
        {
            _tones = tones;
        }

        public Bitmap Apply(Bitmap input)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);

            for(int y = 0; y < input.Height; y++)
            {
                for(int x = 0; x < input.Width; x++)
                {
                    Color inputColor = input.GetPixel(x, y);
                    double luminance = (double)inputColor.R * 0.299 + (double)inputColor.G * 0.587 + (double)inputColor.B * 0.114; // 0 to 255
                    double intervalSize = 256.0 / _tones.Length;
                    int toneIndex = (int)(luminance / intervalSize);
                    Color outputColor = _tones[toneIndex];
                    output.SetPixel(x,y, outputColor);
                }
            }

            return output;
        }
    }
}
