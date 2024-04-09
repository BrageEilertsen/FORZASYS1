// VideoController.cs

using FORZASYS1.Models; // Assuming your Video model is here
using Microsoft.AspNetCore.Mvc;

namespace FORZASYS1.Controllers
{
    public class VideoController : Controller
    {
        private readonly List<Video> _videos; // Simulated video data (optional)
        private readonly ElasticsearchService _elasticsearchService;

        public VideoController(ElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
            // You can remove this if you're not using simulated data
            _videos = new List<Video>
            {
                new Video { Id = 1, Title = "Video 1", Category = "Goals" },
                new Video { Id = 2, Title = "Video 2", Category = "Assists" },
                // Add more videos as needed
            };
        }

        // Action method to retrieve all videos (optional)
        public IActionResult Index()
        {
            return View(_videos); // Use simulated data or remove if not used
        }

        // Action method to retrieve a specific video by ID
        public IActionResult Details(int id)
        {
            var video = _videos.Find(v => v.Id == id); // Use simulated data or remove if not used
            if (video == null)
            {
                return NotFound();
            }
            return View(video);
        }

        // Action method to handle video search
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var results = await _elasticsearchService.SearchHighlights(searchTerm);
                // Use the results to populate your view model
                var model = new VideoListViewModel { Videos = results };
                return View(model);
            }

            return View(); // Display empty view or default message if no search term
        }
    }
}

