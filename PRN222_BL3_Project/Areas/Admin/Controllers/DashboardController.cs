using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_BL3_Project.Models;
using System;
using System.Linq;

namespace PRN222_BL3_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly FootballFieldBookingContext _context;

        public DashboardController(FootballFieldBookingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Revenue(DateTime? fromDate, DateTime? toDate)
        {
            fromDate ??= DateTime.Now.AddMonths(-1);
            toDate ??= DateTime.Now;

            var bookings = _context.Bookings
                .ToList()
                .Where(b => b.BookingDate.ToDateTime(TimeOnly.MinValue) >= fromDate.Value.Date
                            && b.BookingDate.ToDateTime(TimeOnly.MinValue) <= toDate.Value.Date)
                .ToList();
            var totalRevenue = bookings.Sum(b => b.TotalPrice);
            var bookingHistoryViewModels = bookings.Select(b => new BookingHistoryViewModel
            {
                BookingId = b.BookingId,
                BookingDate = b.BookingDate.ToDateTime(TimeOnly.MinValue),
                FieldName = b.Field?.FieldName ?? "",
                TimeSlots = string.Join(", ", b.BookingTimeSlots?.Select(bt => $"{bt.Timeslot?.StartTime:hh\\:mm}-{bt.Timeslot?.EndTime:hh\\:mm}") ?? new string[] { "Không có khung giờ" }),
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                UserName = b.User?.UserName ?? ""
            }).ToList();
            ViewData["TotalRevenue"] = totalRevenue;
            ViewData["FromDate"] = fromDate?.ToString("yyyy-MM-dd");
            ViewData["ToDate"] = toDate?.ToString("yyyy-MM-dd");

            return View(bookingHistoryViewModels);
        }

    }
}
