using System.Drawing;

namespace PersonalWebsite.Filters
{
    public interface IFilter
    {
        public string Name { get; } // name of the filter
        public string Description { get; } // what the filter does

        public Image Filter(Image image);
    }
}
