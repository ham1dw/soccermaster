using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalTickets = await _context.TicketOrders.SumAsync(t => (int?)t.TicketCount) ?? 0;
            ViewBag.TotalBlogs = await _context.News.CountAsync();
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.TotalRevenue = await _context.TicketOrders.Where(t => t.IsPaid).SumAsync(t => (decimal?)t.TotalAmount) ?? 0;

            var recentOrders = await _context.TicketOrders
                .OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View(recentOrders);
        }
    }
}
