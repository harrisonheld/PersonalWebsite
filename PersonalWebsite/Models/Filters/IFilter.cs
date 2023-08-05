using System.Drawing;

namespace PersonalWebsite.Models.Filters
{
    public interface IFilter
    {
        public string Name { get; } // name of the filter
        public string Description { get; } // what the filter does

        public Bitmap Apply(Bitmap image);
    }
}
