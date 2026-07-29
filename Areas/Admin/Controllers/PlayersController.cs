using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Data;
using soccer.Models;
using soccer.Services;
using soccer.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace soccer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PlayersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        private const int PageSize = 10;

        public PlayersController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            var query = _context.Players.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(p => p.Name.Contains(searchString) || p.TeamName.Contains(searchString));
                ViewData["CurrentFilter"] = searchString;
            }

            var list = await PaginatedList<Player>.CreateAsync(query.OrderBy(p => p.Name), pageNumber, PageSize);
            return View(list);
        }

        public IActionResult Create()
        {
            return View(new Player());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var url = await _fileService.SaveImageAsync(imageFile, "uploads");
                    player.ImageUrl = url;
                }

                _context.Players.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();
            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Player player, IFormFile imageFile)
        {
            if (id != player.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        var url = await _fileService.SaveImageAsync(imageFile, "uploads");
                        player.ImageUrl = url;
                    }

                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var player = await _context.Players.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (player == null) return NotFound();
            return View(player);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var player = await _context.Players.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (player == null) return NotFound();
            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id) => _context.Players.Any(e => e.Id == id);
    }
}
