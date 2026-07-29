using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Data;
using soccer.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace soccer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TicketOrdersController : Controller
    {
        private readonly AppDbContext _context;

        public TicketOrdersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.TicketOrders.OrderByDescending(t => t.CreatedAt).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.TicketOrders.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> ExportCsv()
        {
            var list = await _context.TicketOrders.OrderByDescending(t => t.CreatedAt).ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Id,MatchName,Category,FullName,TicketCount,TotalAmount,IsPaid,CreatedAt");
            foreach(var t in list)
            {
                sb.AppendLine($"{t.Id},\"{t.MatchName}\",\"{t.Category}\",\"{t.FullName}\",{t.TicketCount},{t.TotalAmount},{t.IsPaid},{t.CreatedAt:O}");
            }
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "ticketorders.csv");
        }
    }
}
