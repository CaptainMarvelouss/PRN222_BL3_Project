using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Newtonsoft.Json;
using ProjectPRN222_BL3_Project.Services;
using ProjectPRN222_BL3_Project.Helper;
using Repositories;

namespace ProjectPRN222_BL3_Project.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly IBookingRepository _bookingRepository;
        
        public PaymentController(IVnPayService vnPayService, IBookingRepository bookingRepository)

        {
            _vnPayService = vnPayService;
            _bookingRepository = bookingRepository;
        }

        [HttpPost]
        public IActionResult Checkout(string fieldId, decimal priceField, List<int> selectedTimes)
        {
            if (selectedTimes == null || !selectedTimes.Any())
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một khung giờ để thanh toán.";
                return RedirectToAction("Booking", "Booking");
            }

            var orderId = new Random().Next(100000, 999999);
            var totalAmount = selectedTimes.Count * priceField;

            Console.WriteLine("Total Amount: " + totalAmount);
            
            var paymentRequest = new VnPaymentRequestModel
            {
                OrderId = orderId,
                FullName = User.Identity?.Name ?? "Khách vãng lai",
                Description = $"Thanh toán đặt sân #{orderId}",
                Amount = (double)totalAmount,
                CreatedDate = DateTime.Now
            };

            HttpContext.Session.SetString("OrderId", orderId.ToString());
            HttpContext.Session.SetString("FieldId", fieldId);
            HttpContext.Session.SetString("PriceField", priceField.ToString());
            HttpContext.Session.SetString("SelectedTimes", string.Join(",", selectedTimes));
            var filterdate = DateTime.Parse(HttpContext.Session.GetString("SelectedDate"));
            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest);
            return Redirect(paymentUrl);
        }

         

        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                var orderId = HttpContext.Session.GetString("OrderId");
                var fieldId = HttpContext.Session.GetString("FieldId");
                var selectedDate = DateTime.Parse(HttpContext.Session.GetString("SelectedDate"));
                var priceField = decimal.Parse(HttpContext.Session.GetString("PriceField"));
                var selectedTimes = HttpContext.Session.GetString("SelectedTimes")
                                      .Split(',').Select(int.Parse).ToList();
                var userId = User.FindFirst("userId")?.Value;
                Booking total = new Booking
                {
                    UserId = Int32.Parse(userId),
                    FieldId = Int32.Parse(fieldId),
                    BookingDate = DateOnly.FromDateTime(selectedDate),
                    Status = "Booked",
                    TotalPrice = selectedTimes.Count * priceField,
                };

                total = await _bookingRepository.CreateBooking(total);
                for (int i = 0; i < selectedTimes.Count; i++)
                {
                    BookingTimeSlot invoicePitch = new BookingTimeSlot
                    {
                        FieldId = Int32.Parse(fieldId),
                        BookingId = total.BookingId,
                        TimeslotId = selectedTimes[i],
                        BookingDate = DateOnly.FromDateTime(selectedDate)
                    };

                    await _bookingRepository.CreateBookingTimeSlot(invoicePitch);

                }

                HttpContext.Session.Remove("OrderId");
                HttpContext.Session.Remove("FieldId");
                HttpContext.Session.Remove("SelectedDate");
                HttpContext.Session.Remove("PriceField");
                HttpContext.Session.Remove("SelectedTimes");
                TempData["FieldId"] = fieldId;
                TempData["SelectedDate"] = DateOnly.FromDateTime(selectedDate).ToString("yyyy-MM-dd"); 
                TempData["PriceField"] = priceField.ToString();
                TempData["SelectTime"] = selectedTimes.Count;
                List<String> list = new List<String>();
                /*for (int i = 0; i < selectedTimes.Count; i++)
                {
                    string booktime = await _bookingRepository.GetTimeByBookingTimeId(selectedTimes[i]);
                    list.Add(booktime);
                }*/
                TempData["BookingTimes"] = string.Join(",", list);

                return RedirectToAction("ViewInvoice", "Payment");
            }
            else
            {
                TempData["ErrorMessage"] = "Thanh toán thất bại. Vui lòng thử lại.";
                return RedirectToAction("Error");
            }
        }
        
            public IActionResult ViewInvoice()
            {
                return View();
            }
        
    }
}
