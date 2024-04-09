using Microsoft.AspNetCore.Mvc;

namespace FORZASYS1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Logic to retrieve data or perform other actions
            return View();
        }
        
        // Other action methods for different views or actions
    }
}