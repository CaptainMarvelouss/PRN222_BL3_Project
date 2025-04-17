using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Repositories;

namespace PRN222_BL3_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FootballFieldController : Controller
    {
        private readonly IFootballFieldRepository _context;

        public FootballFieldController(IFootballFieldRepository context)
        {
            _context = context;
        }

        // GET: Admin/FootballField
        public async Task<IActionResult> Index()
        {
            var footballFields = _context.GetFootballFields();
            return View(footballFields);
        }

        // GET: Admin/FootballField/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/FootballField/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Field field)
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

            if (ModelState.IsValid)
            {
                _context.AddFootballField(field);
                TempData["SuccessMessage"] = "Field created successfully.";
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
        public async Task<IActionResult> Edit(int id, Field field)
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.UpdateFootballField(field);
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
                _context.DeleteFootballField(field);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FieldExists(int id)
        {
            return _context.GetFootballFields().Any(e => e.FieldId == id);
        }
    }
}
