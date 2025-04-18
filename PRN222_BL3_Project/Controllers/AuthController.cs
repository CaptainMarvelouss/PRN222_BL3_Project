using BusinessObjects;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace PRN222_BL3_Project.Controllers
{
    public class AuthController : Controller
    {
        private readonly FootballFieldBookingContext _context;
        public AuthController(FootballFieldBookingContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ExternalLogin(string provider, string? returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { ReturnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl, string? remoteError)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (info.Principal == null)
            {
				ModelState.AddModelError("", "Authentication failed. Please try again.");
				return RedirectToAction(nameof(Login));
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
            var googleId = info.Principal.FindFirstValue("sub") ??
                   info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    UserName = (firstName + lastName)?.Trim() ?? email,
                    GoogleId = googleId,
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            if (user.IsBlocked == true)
            {
                TempData["BlockMessage"] = "Your account is blocked. Please contact the administrator.";
                return RedirectToAction(nameof(Login));
            }

            var claims = new List<Claim>
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("roleId", user.RoleId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.RoleId switch
                {
                    1 => "Admin",
                    2 => "Staff",
                    3 => "User",
                    _ => "User" // Default
				})
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return user.RoleId switch
            {
                1 => RedirectToAction("Index", "Dashboard", new { area = "Admin" }),
                2 => RedirectToAction("Index", "Dashboard", new { area = "Admin" }),
                3 => RedirectToAction("Index", "Home"),
                _ => RedirectToAction("Index", "Home") // Default User
            };
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
