using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FORZASYS1.Models;
using FORZASYS1.Interfaces;

namespace FORZASYS1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IElasticsearchService _elasticsearchService;

        public HomeController(IElasticsearchService elasticsearchService)
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
                model.Videos = new List<Video>();
            }

            return View(model);
        }
    }
}