﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Drawing.Imaging;

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

        public IEnumerable<Type> FilterTypes { get; set; }

        [BindProperty]
        public IFormFile OriginalImage { get; set; }
    }
}