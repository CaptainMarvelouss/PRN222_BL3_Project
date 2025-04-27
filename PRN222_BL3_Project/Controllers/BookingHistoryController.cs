using Microsoft.AspNetCore.Mvc;
using PRN222_BL3_Project.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PRN222_BL3_Project.Controllers
{
    public class BookingHistoryController : Controller
    {
        private readonly IBookingRepository _bookingRepo;

        public BookingHistoryController(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userId))
            {
                // Handle the case when userId is not found (user not authenticated)
                return RedirectToAction("Login", "Auth");
            }

            // Convert userId to the appropriate type (e.g., int)
            var userIdInt = Convert.ToInt32(userId);

            // Fetch the booking history for the user
            var bookings = _bookingRepo.GetBookingHistoryByUserId(userIdInt);

            // Map Booking to BookingHistoryViewModel
            var bookingHistoryViewModels = bookings.Select(b => new BookingHistoryViewModel
            {
                BookingId = b.BookingId,
                BookingDate = b.BookingDate.ToDateTime(TimeOnly.MinValue),
                FieldName = b.Field.FieldName,
                TimeSlots = string.Join(", ",
                        b.BookingTimeSlots!
                         .Select(bs => $"{bs.Timeslot.StartTime:hh\\:mm}-{bs.Timeslot.EndTime:hh\\:mm}")),
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                UserName = b.User.UserName // Only if needed for Admin/Staff view
            }).ToList();

            return View(bookingHistoryViewModels);
        }

    }
}