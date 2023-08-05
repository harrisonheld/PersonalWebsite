﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Drawing;
using System.IO;
using PersonalWebsite.Models;
using PersonalWebsite.Models.Filters;
using System.Drawing.Imaging;

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
        public IActionResult ApplyFilters([FromForm] IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image provided.");
            }

            using (var image = new Bitmap(imageFile.OpenReadStream()))
            {
                var filters = new IFilter[] { 
                    new Scale(128, 128),
                    new ToneMap(new Color[] { Color.Black, Color.FromArgb(255, 64, 64, 64), Color.FromArgb(255, 192, 192, 192), Color.White }),
                    new Tint(Color.DarkBlue, 0.4f)
                };
                Bitmap output = image;
                foreach(IFilter filter in filters)
                {
                    output = filter.Apply(output);
                }

                using (var outputImageStream = new MemoryStream())
                {
                    // Save the image using the specified encoder
                    output.Save(outputImageStream, ImageFormat.Png);
                    var contentType = $"image/{output.RawFormat.ToString().ToLower()}";

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