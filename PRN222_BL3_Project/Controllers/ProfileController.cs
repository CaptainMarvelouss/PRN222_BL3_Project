using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace PRN222_BL3_Project.Controllers
{
    public class ProfileController : Controller
    {
        private readonly FootballFieldBookingContext _context;

        public ProfileController(FootballFieldBookingContext context)
        {
            _context = context;
        }

        // GET: Profile/Index
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst("userId")?.Value;
                if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                {
                    var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                    if (user != null)
                    {
                        return View(user);
                    }
                }
            }
            return RedirectToAction("Login", "Auth");
        }

        // GET: Profile/Update
        public IActionResult Update()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Login", "Auth");
        }

        // POST: Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst("userId")?.Value;
                if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                {
                    var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                    if (user != null)
                    {
                        user.UserName = updatedUser.UserName;
                        user.Phone = updatedUser.Phone;
                        user.Email = updatedUser.Email;
                        user.PasswordHash = updatedUser.PasswordHash;
                        _context.SaveChanges();

                        TempData["SuccessMessage"] = "Profile updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            TempData["ErrorMessage"] = "Error updating profile.";
            return View(updatedUser);
        }
    }
}
