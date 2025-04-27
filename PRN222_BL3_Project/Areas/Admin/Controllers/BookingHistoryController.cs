using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_BL3_Project.Models;
using Repositories;

namespace PRN222_BL3_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class BookingHistoryController : Controller
    {
        private readonly IBookingRepository _bookingRepo;

        public BookingHistoryController(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        // GET: /Admin/BookingHistory
        public IActionResult Index(string? keyword, DateTime? fromDate, DateTime? toDate, int currentPage = 1)
        {
            // Chuyển DateTime? thành DateOnly?
            DateOnly? from = fromDate.HasValue
                ? DateOnly.FromDateTime(fromDate.Value)
                : null;
            DateOnly? to = toDate.HasValue
                ? DateOnly.FromDateTime(toDate.Value)
                : null;

            // Set the page size for pagination
            int pageSize = 10;

            // Fetch the total number of records
            var totalRecords = _bookingRepo
                .SearchBookings(
                    userId: null,
                    keyword: keyword,
                    fromDate: from,
                    toDate: to)
                .Count();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Get the records for the current page
            var bookings = _bookingRepo
                .SearchBookings(
                    userId: null,
                    keyword: keyword,
                    fromDate: from,
                    toDate: to)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookingHistoryViewModel
                {
                    BookingDate = b.BookingDate.ToDateTime(TimeOnly.MinValue),
                    // Dùng UserName chứ không phải FullName
                    UserName = b.User!.UserName!,
                    // Dùng FieldName chứ không phải Name
                    FieldName = b.Field!.FieldName,
                    // Dùng Timeslot.StartTime / EndTime
                    TimeSlots = string.Join(", ",
                        b.BookingTimeSlots!
                         .Select(bs => $"{bs.Timeslot.StartTime:hh\\:mm}-{bs.Timeslot.EndTime:hh\\:mm}")),
                    TotalPrice = b.TotalPrice,
                    Status = b.Status!
                });

            // Pass the necessary data to the view
            ViewData["Keyword"] = keyword;
            ViewData["FromDate"] = fromDate?.ToString("yyyy-MM-dd");
            ViewData["ToDate"] = toDate?.ToString("yyyy-MM-dd");
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;

            return View(bookings);
        }
    }
}
