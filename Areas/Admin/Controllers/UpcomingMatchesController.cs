using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Data;
using soccer.Models;
using soccer.Services;
using System.Threading.Tasks;

namespace soccer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UpcomingMatchesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public UpcomingMatchesController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.UpcomingMatches.OrderBy(m => m.Date).ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View(new UpcomingMatch { Date = System.DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UpcomingMatch model, Microsoft.AspNetCore.Http.IFormFile team1Logo, Microsoft.AspNetCore.Http.IFormFile team2Logo)
        {
            if (ModelState.IsValid)
            {
                if (team1Logo != null) model.Team1LogoUrl = await _fileService.SaveImageAsync(team1Logo, "uploads");
                if (team2Logo != null) model.Team2LogoUrl = await _fileService.SaveImageAsync(team2Logo, "uploads");
                _context.UpcomingMatches.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.UpcomingMatches.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpcomingMatch model, Microsoft.AspNetCore.Http.IFormFile team1Logo, Microsoft.AspNetCore.Http.IFormFile team2Logo)
        {
            if (id != model.id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    if (team1Logo != null) model.Team1LogoUrl = await _fileService.SaveImageAsync(team1Logo, "uploads");
                    if (team2Logo != null) model.Team2LogoUrl = await _fileService.SaveImageAsync(team2Logo, "uploads");
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.UpcomingMatches.AnyAsync(m => m.id == model.id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.UpcomingMatches.AsNoTracking().FirstOrDefaultAsync(m => m.id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.UpcomingMatches.AsNoTracking().FirstOrDefaultAsync(m => m.id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.UpcomingMatches.FindAsync(id);
            if (item != null)
            {
                _context.UpcomingMatches.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
