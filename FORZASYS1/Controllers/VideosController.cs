using Microsoft.AspNetCore.Mvc;
using FORZASYS1.Interfaces;
using System.Threading.Tasks;

namespace FORZASYS1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly IElasticsearchService _elasticsearchService;

        public VideosController(IElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVideos([FromQuery] string searchTerm)
        {
            var videos = await _elasticsearchService.SearchHighlights(searchTerm);
            return Ok(videos);
        }
    }
}