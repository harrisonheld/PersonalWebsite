﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Filters;
using System.Drawing;

namespace PersonalWebsite.Pages
{
    public class ImageFilteringModel : PageModel
    {
        private readonly ILogger<ImageFilteringModel> _logger;
        private readonly IFilterDiscoveryService _filterDiscoveryService;

        public ImageFilteringModel(ILogger<ImageFilteringModel> logger, IFilterDiscoveryService filterDiscoveryService)
        {
            _logger = logger;
            _filterDiscoveryService = filterDiscoveryService;
        }

        public IActionResult OnGet()
        {
            FilterTypes = _filterDiscoveryService.GetFilterImplementations();

            return Page();
        }

        public void OnPost()
        {
            Scale scale = new Scale();
            scale.OutputWidth = 64;
            scale.OutputHeight = 64;

            Image input = Image.FromStream(OriginalImage.OpenReadStream());
            Image output = scale.Filter(input);
        }

        public IEnumerable<Type> FilterTypes { get; set; }

        [BindProperty]
        public IFormFile OriginalImage { get; set; }
        [BindProperty]
        public IFilter[] Filters { get; set; }
    }
}