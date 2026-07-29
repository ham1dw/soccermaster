using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Data;
using soccer.Models;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MatchesController : Controller
    {
        private readonly AppDbContext _context;

        public MatchesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Matches
        public async Task<IActionResult> Index()
        {
            var matches = await _context.Matches.AsNoTracking().ToListAsync();
            return View(matches);
        }

        // GET: Admin/Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);
            if (match == null) return NotFound();
            return View(match);
        }

        // GET: Admin/Matches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Matches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeTeam,HomeResult,HomeScore,AwayScore,AwayTeam,AwayResult,HomePlayer1,HomePlayer2,HomePlayer3,HomePlayer4,AwayPlayer1,AwayPlayer2,AwayPlayer3,AwayPlayer4,Team1LogoUrl,Team2LogoUrl")] Match match)
        {
            if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        // GET: Admin/Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var match = await _context.Matches.FindAsync(id);
            if (match == null) return NotFound();
            return View(match);
        }

        // POST: Admin/Matches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HomeTeam,HomeResult,HomeScore,AwayScore,AwayTeam,AwayResult,HomePlayer1,HomePlayer2,HomePlayer3,HomePlayer4,AwayPlayer1,AwayPlayer2,AwayPlayer3,AwayPlayer4,Team1LogoUrl,Team2LogoUrl")] Match match)
        {
            if (id != match.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        // GET: Admin/Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);
            if (match == null) return NotFound();
            return View(match);
        }

        // POST: Admin/Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
