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
    public class HeroSectionsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public HeroSectionsController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.HeroSections.AsNoTracking().ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View(new HeroSection());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HeroSection model, Microsoft.AspNetCore.Http.IFormFile bgFile)
        {
            if (ModelState.IsValid)
            {
                if (bgFile != null)
                {
                    var url = await _fileService.SaveImageAsync(bgFile, "uploads");
                    model.BackgroundImage = url;
                }
                _context.HeroSections.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.HeroSections.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HeroSection model, Microsoft.AspNetCore.Http.IFormFile bgFile)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    if (bgFile != null)
                    {
                        var url = await _fileService.SaveImageAsync(bgFile, "uploads");
                        model.BackgroundImage = url;
                    }
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.HeroSections.AnyAsync(h => h.Id == model.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.HeroSections.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.HeroSections.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.HeroSections.FindAsync(id);
            if (item != null)
            {
                _context.HeroSections.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
