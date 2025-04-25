using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Repositories;
using Microsoft.AspNetCore.Authorization;
[Authorize(Roles = "User")]
public class BookingController : Controller
{
    private readonly FootballFieldBookingContext _context;
    //private readonly IBookingRepository _bookingRepository;
    public BookingController(FootballFieldBookingContext context)
    {
        _context = context;
        //_bookingRepository = bookingRepository;
    }


    [HttpPost]
    public IActionResult Booking(int fieldId, DateOnly selectedDate)
    {
        var availableSlots = _context.TimeSlots
            .Where(slot => !_context.BookingTimeSlots
                .Any(b => b.FieldId == fieldId && b.BookingDate == selectedDate && b.TimeslotId == slot.TimeslotId))
            .ToList();

        ViewBag.AvailableTimeSlots = availableSlots;
        ViewBag.SelectedDate = selectedDate;
        ViewBag.FieldId = fieldId;
        var price = _context.Fields
            .Where(f => f.FieldId == fieldId)
            .FirstOrDefault().Price;
        ViewBag.priceField = price;
        HttpContext.Session.SetString("SelectedDate", selectedDate.ToString());

        return View();  
        //return Json(availableSlots);
    }



}
