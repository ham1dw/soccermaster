using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Models;
using soccer.Data;
using System.Diagnostics;
using System.Threading.Tasks;


namespace soccer.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hero = await _context.HeroSections.FirstOrDefaultAsync();
            var matches = await _context.Matches.AsNoTracking().ToListAsync();

            matches = matches
                .GroupBy(m => new { m.HomeTeam, m.AwayTeam, m.HomeScore, m.AwayScore })
                .Select(g => g.OrderBy(x => x.Id).First())
                .ToList(); 

            var model = new HomeViewModel
            {
                Hero = hero,
                Matches = matches,
                LastMatch = await _context.Matches
                                .OrderByDescending(m => m.Id)
                                .FirstOrDefaultAsync(),
                Standings = await _context.Standings
                                .OrderBy(s => s.Position)
                                .ToListAsync(),
                News = await _context.News.ToListAsync()
            };

            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}