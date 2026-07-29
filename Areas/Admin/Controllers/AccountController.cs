using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using soccer.Models.ViewModels;
using soccer.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace soccer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            _logger.LogInformation("[LOGIN] Attempt: Email='{Email}', ModelState.IsValid={IsValid}", model?.Email, ModelState.IsValid);

            // Log form keys for debugging
            try
            {
                if (Request.HasFormContentType)
                {
                    var keys = Request.Form.Keys.ToList();
                    _logger.LogInformation("[LOGIN] Request.Form keys: {Keys}", string.Join(',', keys));
                    foreach (var k in keys)
                    {
                        _logger.LogInformation("[LOGIN] Form[{Key}]='{Value}'", k, Request.Form[k].ToString());
                    }
                }
                else
                {
                    _logger.LogInformation("[LOGIN] Request has no form content");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read Request.Form");
            }

            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                    foreach (var err in kvp.Value.Errors)
                        System.Console.WriteLine($"[LOGIN] Validasiya xetasi [{kvp.Key}]: {err.ErrorMessage}");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            System.Console.WriteLine($"[LOGIN] Istifadeci tapildi: {(user != null)}");
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            System.Console.WriteLine($"[LOGIN] Netice: Succeeded={result.Succeeded}, IsLockedOut={result.IsLockedOut}, IsNotAllowed={result.IsNotAllowed}, RequiresTwoFactor={result.RequiresTwoFactor}");
            if (result.Succeeded)
            {
                System.Console.WriteLine("[LOGIN] Ugurlu giris, Dashboard-a yonlendirilir.");
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
