using Microsoft.AspNetCore.Mvc;

namespace soccer.Controllers
{
    public class contact : Controller
    {
        public IActionResult Index()
        {
            return View("/Views/Home/contact.cshtml");
        }
    }
}
