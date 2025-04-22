using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Repositories.IRepositories;

namespace PRN222_BL3_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _context;
        private readonly FootballFieldBookingContext _role;

        public UsersController(IUserRepository context, FootballFieldBookingContext role)
        {
            _context = context;
            _role = role;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var users = _context.GetUsers().ToList();
            return View(users);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_role.Roles, "RoleId", "RoleName");
            return View();
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.AddUser(user);
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_role.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_role.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.UpdateUser(user);
                    TempData["UpdateUserSuccess"] = $"User [{user.UserName}] has been updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_role.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // POST: Admin/Users/Block/5
        [HttpGet]
        public async Task<IActionResult> Block(int id)
        {
            try
            {
                var user = _context.GetUserById(id);
                if(user == null)
                {
                    throw new Exception("User not found.");
                }
                _context.BlockUser(id);
                TempData["UpdateUserSuccess"] = $"User [{user.UserName}] has been blocked successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error blocking user: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Users/Unblock/5
        [HttpGet]
        public async Task<IActionResult> Unblock(int id)
        {
            try
            {
                var user = _context.GetUserById(id);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }
                _context.UnblockUser(id);
                TempData["UpdateUserSuccess"] = $"User [{user.UserName}] has been unblocked successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error unblocking user: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.GetUsers().Any(e => e.UserId == id);
        }
    }
}
