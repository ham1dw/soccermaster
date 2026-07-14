using Microsoft.AspNetCore.Mvc;
using soccer.Data;
using Microsoft.EntityFrameworkCore;

namespace soccer.Controllers
{
    public class MatchesController : Controller
    {
        private readonly AppDbContext _context;
        public MatchesController(AppDbContext context) { _context = context; }

        public IActionResult Index()
        {
            var matches = _context.Matches.AsNoTracking().ToList();

       
            matches = matches
                .GroupBy(m => new { m.HomeTeam, m.AwayTeam, m.HomeScore, m.AwayScore })
                .Select(g => g.OrderBy(m => m.Id).First())
                .ToList();

            return View("~/Views/Home/matches.cshtml", matches);
        }
    }
}
