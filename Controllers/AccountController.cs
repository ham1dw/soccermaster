using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using soccer.DTOs;
using soccer.Models;
using System.Linq;
using System.Threading.Tasks;

namespace soccer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["AccountError"] = "Lutfen melumatlari duzgun daxil edin!";
                return RedirectToAction("Index", "Home");
            }

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["AccountSuccess"] = "Qeydiyyat ugurla tamamlandi!";
                return RedirectToAction("Index", "Home");
            }

            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            TempData["AuthError"] = errors;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            _logger.LogInformation("[LOGIN] Attempt: Email='{Email}' ModelState.IsValid={IsValid}", dto?.Email, ModelState.IsValid);

            if (!ModelState.IsValid)
            {
                
                try
                {
                    if (Request.HasFormContentType)
                    {
                        var keys = Request.Form.Keys.ToList();
                        _logger.LogWarning("[LOGIN] ModelState invalid. Form keys: {Keys}", string.Join(',', keys));
                        foreach (var k in keys)
                        {
                            _logger.LogWarning("[LOGIN] Form[{Key}]='{Value}'", k, Request.Form[k].ToString());
                        }
                    }
                    else
                    {
                        _logger.LogWarning("[LOGIN] No form content in request");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to log form values");
                }

                TempData["AuthError"] = "Email ve sifre bos saxlanila bilmez!";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                TempData["AuthError"] = "Bele bir hesab tapilmadi. Zehmet olmasa once qeydiyyatdan kecin!";
                return RedirectToAction("Index", "Home");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName ?? user.Email, dto.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["AccountSuccess"] = "Ugurla daxil oldunuz!";
                return RedirectToAction("Index", "Home");
            }

           
            TempData["AuthError"] = "Daxil edilen sifre yanlisdir!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["AccountError"] = "Zehmet olmasa butun xanalari duzgun doldurun!";
                return RedirectToAction("Blog", "Blog");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["AccountError"] = "Istifadeci tapilmadi!";
                return RedirectToAction("Blog", "Blog");
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["AccountSuccess"] = "Sifreniz ugurla yenilendi!";
            }
            else
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                TempData["AccountError"] = errors;
            }

            return RedirectToAction("Blog", "Blog");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["AccountSuccess"] = "Ugurla cixis edildi.";
            return RedirectToAction("Index", "Home");
        }
    }
}