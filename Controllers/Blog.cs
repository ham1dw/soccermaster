using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Data;

namespace soccer.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Blog(int? id)
        {
            
            var newsList = await _context.News.ToListAsync();

            
            if (id.HasValue)
            {
                
                var selectedNews = newsList.FirstOrDefault(n => n.Id == id.Value);

                if (selectedNews != null)
                {
                    
                    newsList.Remove(selectedNews);
                    newsList.Insert(0, selectedNews);
                }
            }

            
            return View("~/Views/Blog/blog.cshtml", newsList);
        }
    }
}