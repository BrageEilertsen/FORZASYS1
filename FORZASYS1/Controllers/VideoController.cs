// VideoController.cs
using FORZASYS1.Models; // Assuming your Video model is here
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FORZASYS1.Controllers
{
    public class VideoController : Controller
    {
        private readonly ElasticsearchService _elasticsearchService;

        public VideoController(ElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        // Action method to retrieve all videos (optional)
        public IActionResult Index()
        {
            // Assuming _videos is no longer used here directly
            return View(); // If you still need to use _videos, ensure it's properly initialized
        }

        // Action method to handle video search
        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var model = new VideoListViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var results = await _elasticsearchService.SearchHighlights(searchTerm);
                foreach (var video in results)
                {
                    video.Url = _elasticsearchService.ConstructBlobUrl(video.Id);  // Construct the URL using the service
                    Console.WriteLine($"Video URL: {video.Url}");  // Log for verification
                }
                model.Videos = results;
            }

            return View(model);  // Display the search results
        }
    }
}