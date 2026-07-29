using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Data;
using soccer.Models;
using soccer.Services;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace soccer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public NewsController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.News.OrderByDescending(n => n.Date).ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View(new NewsItem { Date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsItem item, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var url = await _fileService.SaveImageAsync(imageFile, "uploads");
                    item.ImageUrl = url;
                }
                _context.News.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.News.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsItem item, IFormFile imageFile)
        {
            if (id != item.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        var url = await _fileService.SaveImageAsync(imageFile, "uploads");
                        item.ImageUrl = url;
                    }
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.News.Any(n => n.Id == item.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.News.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.News.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.News.FindAsync(id);
            if (item != null)
            {
                _context.News.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Image upload endpoint for rich text editor (TinyMCE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            var url = await _fileService.SaveImageAsync(file, "uploads");
            if (string.IsNullOrEmpty(url)) return StatusCode(500);

            return Json(new { location = url });
        }
    }
}
