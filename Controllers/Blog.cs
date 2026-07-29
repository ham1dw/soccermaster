using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soccer.Data;
using soccer.Models;
using System.Globalization;

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
        [HttpGet]
        public IActionResult BookTicket()
        {
           
            if (!User.Identity.IsAuthenticated)
            {
                TempData["AuthError"] = "Bilet almaq ucun evvelce daxil olmalisiniz!";
                return RedirectToAction("Index", "Home");
            }

            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> BookTicket(TicketOrder order, string CardNumber, string ExpiryDate, string CVV)
        {
            if (ModelState.IsValid)
            {
                decimal pricePerTicket = 0;

                if (order.Category == "Standart") pricePerTicket = 10;
                else if (order.Category == "Fan Zone") pricePerTicket = 20;
                else if (order.Category == "VIP") pricePerTicket = 50;

                // DUZELIS: Toplama (+) yox, Vurma (*) olmalidir
                order.TotalAmount = pricePerTicket * order.TicketCount;

                if (!string.IsNullOrEmpty(CardNumber) && !string.IsNullOrEmpty(ExpiryDate) && !string.IsNullOrEmpty(CVV))
                {
                    order.IsPaid = true;
                }
                else
                {
                    TempData["ErrorMessage"] = "Kart melumatlarini duzgun daxil edin zehmet olmasa.";
                    return RedirectToAction("Blog");
                }

                _context.TicketOrders.Add(order);
                await _context.SaveChangesAsync();

                TempData["SuccesMessage"] = $"Sifaris tesdiqlendi! Odenilen mebleg: {order.TotalAmount} AZN";
                return RedirectToAction("Blog");
            }

            TempData["ErrorMessage"] = "Xahis olunur xanalari duzgun doldurun.";
            return RedirectToAction("Blog");
        }

    }
    }
