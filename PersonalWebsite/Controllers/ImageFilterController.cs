using Microsoft.AspNetCore.Mvc;
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
                var filters = new IFilter[] { new Grayscale() };
                Image output = image;
                foreach(IFilter filter in filters)
                {
                    output = filter.Apply(output);
                }

                using (var outputImageStream = new MemoryStream())
                {
                    // Specify the encoder for the image format (e.g., JPEG)
                    var jpegEncoder = ImageCodecInfo.GetImageEncoders().FirstOrDefault(e => e.FormatID == ImageFormat.Jpeg.Guid);

                    // Save the image using the specified encoder
                    output.Save(outputImageStream, jpegEncoder, null);
                    var contentType = $"image/{output.RawFormat.ToString().ToLower()}";

                    return File(outputImageStream.ToArray(), contentType);
                }
            }
        }
    }
}