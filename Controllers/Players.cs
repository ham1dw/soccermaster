using Microsoft.AspNetCore.Mvc;

namespace soccer.Controllers
{
    public class Players : Controller
    {
        public IActionResult Index()
        {
            return View("/Views/Home/players.cshtml");
        }
    }
}
