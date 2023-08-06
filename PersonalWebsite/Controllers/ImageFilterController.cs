using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Drawing;
using System.IO;
using PersonalWebsite.Models;
using PersonalWebsite.Models.Filters;
using System.Drawing.Imaging;
using System.Reflection;

namespace PersonalWebsite.Controllers
{
    [Route("api/imagefilter/")]
    [ApiController]
    public class ImageFilterController : ControllerBase
    {
        private readonly IFilterDiscoveryService _filterDiscoveryService;

        public ImageFilterController(IFilterDiscoveryService filterDiscoveryService)
        {
            _filterDiscoveryService = filterDiscoveryService;
        }

        [HttpPost]
        [Route("apply")]
        /// <summary>
        /// FilterTypeNames is an array of strings that are Type names
        /// FilterParameters is an array of parameter arrays
        public IActionResult ApplyFilters([FromForm] IFormFile imageFile, [FromForm] string[] FilterTypeNames, [FromForm] string[] FilterParameters)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image provided.");
            }

            IFilter[] filters = new IFilter[FilterTypeNames.Length];
            int totalParamsCast = 0;
            for (int i = 0; i < filters.Length; i++)
            {
                Type filterType = _filterDiscoveryService.GetFilterTypeByName(FilterTypeNames[i]);

                // get first constructor
                ConstructorInfo constructor = filterType.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).First();
                // cast the params
                ParameterInfo[] constructorParams = constructor.GetParameters();
                object[] convertedParams = new object[constructorParams.Length];
                for (int j = 0; j < constructorParams.Length; j++)
                {
                    Type paramType = constructorParams[j].ParameterType;
                    string paramValueStr = FilterParameters[totalParamsCast++];
                    object paramValue;

                    if(paramType == typeof(Color))
                    {
                        // convert strings of the form #ABCDEF
                        string redHex = paramValueStr.Substring(1, 2);
                        string greenHex = paramValueStr.Substring(3, 2);
                        string blueHex = paramValueStr.Substring(5, 2);

                        byte red = Convert.ToByte(redHex, 16);
                        byte green = Convert.ToByte(greenHex, 16);
                        byte blue = Convert.ToByte(blueHex, 16);

                        paramValue = Color.FromArgb(red, green, blue);
                    }
                    else
                    {
                        paramValue = Convert.ChangeType(paramValueStr, paramType);
                    }

                    convertedParams[j] = paramValue;
                }

                // create the filter
                filters[i] = (IFilter)constructor.Invoke(convertedParams);
            }

            // apply all the filters
            using (var image = new Bitmap(imageFile.OpenReadStream()))
            {
                Bitmap output = image;
                foreach(IFilter filter in filters)
                {
                    output = filter.Apply(output);
                }

                using (var outputImageStream = new MemoryStream())
                {
                    output.Save(outputImageStream, ImageFormat.Png);
                    var contentType = "image/png";

                    return File(outputImageStream.ToArray(), contentType);
                }
            }
        }

        [HttpGet]
        [Route("filterParameters")]
        public BasicParameterInfo[] GetFilterParameters(string filterTypeName)
        {
            Type filterType = _filterDiscoveryService.GetFilterTypeByName(filterTypeName);

            return filterType.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .First()
                .GetParameters()
                .Select(p => new BasicParameterInfo() { Name = p.Name, Type = p.ParameterType.Name } )
                .ToArray();
        }
    }

    public class BasicParameterInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}