using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Data;

namespace soccer.Controllers
{
    public class MatchesController : Controller
    {
        private readonly AppDbContext _context;

        public MatchesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var matches = await _context.Matches.AsNoTracking().ToListAsync();

            ViewBag.NextMatch = await _context.NextMatches.AsNoTracking().FirstOrDefaultAsync();

            
            ViewBag.UpcomingMatches = await _context.UpcomingMatches
                .Where(u => u.Date >= DateTime.Now) 
                .AsNoTracking()
                .ToListAsync(); 

            matches = matches
                .GroupBy(m => new { m.HomeTeam, m.AwayTeam, m.HomeScore, m.AwayScore })
                .Select(g => g.OrderBy(m => m.Id).First())
                .ToList();

            return View("~/Views/Home/matches.cshtml", matches);
        }
    }
}