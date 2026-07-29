using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Models;
using soccer.Data;

namespace soccer.Controllers
{
    public class Players : Controller
    {
        private readonly AppDbContext _context;

        public   Players(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var allPlayers = _context.Players.ToList();

            return View("/Views/Home/players.cshtml", allPlayers);
        }
    }
}
