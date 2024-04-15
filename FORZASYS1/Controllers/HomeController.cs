using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FORZASYS1.Models;

namespace FORZASYS1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ElasticsearchService _elasticsearchService;

        public HomeController(ElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var model = new VideoListViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.Videos = await _elasticsearchService.SearchHighlights(searchTerm);
            }
            else
            {
                // TODO: Provide a method to get default videos when there is no search term.
                // model.Videos = await _elasticsearchService.GetDefaultVideos();
                // For now, let's initialize Videos with an empty list to avoid null reference errors.
                model.Videos = new List<Video>();
            }

            return View(model);
        }
        
        // Other action methods for different views or actions
    }
}