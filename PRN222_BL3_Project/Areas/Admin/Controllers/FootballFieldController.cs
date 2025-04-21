using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Repositories;
using Microsoft.AspNetCore.Authorization;

namespace PRN222_BL3_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FootballFieldController : Controller
    {
        private readonly IFootballFieldRepository _context;

        public FootballFieldController(IFootballFieldRepository context)
        {
            _context = context;
        }

        // GET: Admin/FootballField
        public async Task<IActionResult> Index(int currentPage = 1, int pageSize = 3, string? search = null)
        {
            var footballFields = _context.GetFootballFields().AsQueryable();

            //Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                footballFields = footballFields.Where(u => u.FieldName.Contains(search));

                var searchResultCount = footballFields.Count();
                if (searchResultCount == 0)
                {
                    ViewBag.NoResults = $"No results found for '{search}'.";
                }
            }

            //Pagination
            var itemCount = _context.GetFootballFields().Count();
            var items = footballFields.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            // Pass data to view
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = (int)Math.Ceiling(itemCount / (float)pageSize);
            ViewBag.SearchQuery = search;

            return View(items);
        }

        // GET: Admin/FootballField/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/FootballField/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Field field, IFormFile imageFile)
        {
            if (_context.GetFootballFields().Any(f => f.FieldName == field.FieldName))
            {
                ModelState.AddModelError("FieldName", "A field with this name already exists.");
            }
            if(field.FieldName == null || field.FieldType == null)
            {
                ModelState.AddModelError("", "Name and Type cannot be empty.");
            }
            if (ModelState.TryGetValue("Price", out var priceEntry) && priceEntry.Errors.Any())
            {
                // Model binding failed (e.g., non-numeric input like "abc")
                ModelState.AddModelError("Price", "Price must be a valid number.");
            }
            else if (field.Price < 0)
            {
                ModelState.AddModelError("Price", "Price cannot be negative.");
            }
            if (field.Status == null)
            {
                ModelState.AddModelError("Status", "Please choose a status.");
            }

            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Image is required.");
            }
            else
            {
                var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!validExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Invalid image format. Only .jpg, .jpeg, .png, .gif are allowed.");
                }
            }

            if (ModelState.IsValid)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/uploads/footballFields", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                field.Image = fileName;
                _context.AddFootballField(field);
                TempData["FieldSuccess"] = "Field created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(field);
        }

        // GET: Admin/FootballField/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var field = _context.GetFootballFieldById(id);
            if (field == null)
            {
                return NotFound();
            }
            return View(field);
        }

        // POST: Admin/FootballField/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Field field, IFormFile? imageFile)
        {
            if (id != field.FieldId)
            {
                return NotFound();
            }

            if (_context.GetFootballFields().Any(f => f.FieldId != id && f.FieldName == field.FieldName))
            {
                ModelState.AddModelError("FieldName", "A field with this name already exists.");
            }
            if (field.FieldName == null || field.FieldType == null)
            {
                ModelState.AddModelError("", "Name and Type cannot be empty.");
            }
            if (ModelState.TryGetValue("Price", out var priceEntry) && priceEntry.Errors.Any())
            {
                // Model binding failed (e.g., non-numeric input like "abc")
                ModelState.AddModelError("Price", "Price must be a valid number.");
            }
            else if (field.Price < 0)
            {
                ModelState.AddModelError("Price", "Price cannot be negative.");
            }
            if (field.Status == null)
            {
                ModelState.AddModelError("Status", "Please choose a status.");
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!validExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Invalid image format. Only .jpg, .jpeg, .png, .gif are allowed.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingField = _context.GetFootballFieldById(id);
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(existingField?.Image))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/uploads/footballFields", existingField.Image);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        var extension = Path.GetExtension(imageFile.FileName).ToLower();
                        var fileName = Guid.NewGuid().ToString() + extension;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/uploads/footballFields", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        field.Image = fileName;
                    }
                    else
                    {
                        field.Image = existingField?.Image;
                    }

                    _context.UpdateFootballField(field);
                    TempData["FieldSuccess"] = "Field updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FieldExists(field.FieldId))
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
            return View(field);
        }

        // GET: Admin/FootballField/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var field = _context.GetFootballFieldById(id);
            if (field == null)
            {
                return NotFound();
            }

            return View(field);
        }

        // POST: Admin/FootballField/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var field = _context.GetFootballFieldById(id);
            if (field != null)
            {
                if (!string.IsNullOrEmpty(field.Image))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/uploads/footballFields", field.Image);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.DeleteFootballField(field);
                TempData["FieldSuccess"] = "Field deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FieldExists(int id)
        {
            return _context.GetFootballFields().Any(e => e.FieldId == id);
        }
    }
}
