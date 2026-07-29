using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using soccer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace soccer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var adminIds = new List<string>();
            foreach (var u in users)
            {
                if (await _userManager.IsInRoleAsync(u, "Admin"))
                {
                    adminIds.Add(u.Id);
                }
            }
            ViewBag.AdminIds = adminIds;
            return View(users);
        }

        public async Task<IActionResult> MakeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Istifadeci tapilmadi.";
                return RedirectToAction("Index");
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["Success"] = $"{user.Email} artiq admindir.";
                return RedirectToAction("Index");
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                TempData["Success"] = $"{user.Email} ugurla admin edildi.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Istifadeci tapilmadi.";
                return RedirectToAction("Index");
            }

            if (user.Email == User.Identity.Name)
            {
                TempData["Error"] = "Ozunuzu adminden cixara bilmezsiniz.";
                return RedirectToAction("Index");
            }

            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            if (admins.Count <= 1)
            {
                TempData["Error"] = "Sistemde en az bir admin qalmalidir.";
                return RedirectToAction("Index");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                TempData["Success"] = $"{user.Email} admin rolundan cixarildi.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            }
            return RedirectToAction("Index");
        }


    }
}
