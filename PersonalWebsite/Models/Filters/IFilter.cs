using System.Drawing;

namespace PersonalWebsite.Models.Filters
{
    public interface IFilter
    {
        public Bitmap Apply(Bitmap image);
    }
}
