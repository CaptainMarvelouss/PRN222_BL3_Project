using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Newtonsoft.Json;
using ProjectPRN222_BL3_Project.Services;
using Repositories.IRepositories;
using ProjectPRN222_BL3_Project.Helper;

namespace ProjectPRN222_BL3_Project.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        //private readonly ITotalInvoicePitchRepository totalInvoicePitchRepository;
        private readonly IBookingRepository bookingRepository;
        //private readonly IBookingTimeRepository bookingTimeRepository;
        //private readonly IPitchRepository pitchRepository;
        public PaymentController(IVnPayService vnPayService)//, ITotalInvoicePitchRepository totalInvoicePitchRepo,
        //IInvoicePitchRepository invoicePitchRepo,
        //IBookingTimeRepository bookingTimeRepo,
        //IPitchRepository pitchRepo)
        {
            _vnPayService = vnPayService;
            /*totalInvoicePitchRepository = totalInvoicePitchRepo;
            invoicePitchRepository = invoicePitchRepo;
            bookingTimeRepository = bookingTimeRepo;
            pitchRepository = pitchRepo;*/
        }

        [HttpPost]
        public IActionResult Checkout(string pitchId, decimal pricePitch, List<int> selectedTimes)
        {
            if (selectedTimes == null || !selectedTimes.Any())
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một khung giờ để thanh toán.";
                return RedirectToAction("Details", "Pitches", new { id = pitchId });
            }

            var orderId = new Random().Next(100000, 999999);
            var totalAmount = selectedTimes.Count * pricePitch;

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
            HttpContext.Session.SetString("PitchId", pitchId);
            
            HttpContext.Session.SetString("PricePitch", pricePitch.ToString());
            HttpContext.Session.SetString("SelectedTimes", string.Join(",", selectedTimes));
            var filterdate = DateTime.Parse(HttpContext.Session.GetString("FilterDate"));
            Console.WriteLine("Filter dâte " + filterdate);
            Console.WriteLine("Redirecting to payment...");
            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest);
            return Redirect(paymentUrl);
        }



        /*public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                var orderId = HttpContext.Session.GetString("OrderId");
                var pitchId = HttpContext.Session.GetString("PitchId");
                var selectedDate = DateTime.Parse(HttpContext.Session.GetString("FilterDate"));
                var pricePitch = decimal.Parse(HttpContext.Session.GetString("PricePitch"));
                var selectedTimes = HttpContext.Session.GetString("SelectedTimes")
                                      .Split(',').Select(int.Parse).ToList();

                for (int i = 0; i < selectedTimes.Count; i++)
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    TotalInvoicePitch total = new TotalInvoicePitch
                    {
                        UserId = Int32.Parse(userId),
                        BookTime = DateOnly.FromDateTime(selectedDate)
                    };
                    totalInvoicePitchRepository.CreateTotalInvoice(total);

                    InvoicePitch invoicePitch = new InvoicePitch
                    {
                        PitchId = pitchId,
                        PricePitchId = pitchRepository.GetPricePitchIdByPitchId(pitchId),
                        TotalInvoiceId = total.TotalInvoiceId,
                        BookingTimeId = selectedTimes[i]
                    };

                    invoicePitchRepository.CreateInvoicePitch(invoicePitch);

                }

                HttpContext.Session.Remove("OrderId");
                HttpContext.Session.Remove("PitchId");
                HttpContext.Session.Remove("FilterDate");
                HttpContext.Session.Remove("PricePitch");
                HttpContext.Session.Remove("SelectedTimes");
                TempData["PitchId"] = pitchId;
                TempData["SelectedDate"] = DateOnly.FromDateTime(selectedDate).ToString("yyyy-MM-dd"); 
                TempData["PricePitch"] = pricePitch.ToString();
                List<String> list = new List<String>();
                for (int i = 0; i < selectedTimes.Count; i++) { 
                    string booktime = bookingTimeRepository.GetTimeByBookingTimeId(selectedTimes[i]);
                    list.Add(booktime);
                }
                TempData["BookingTimes"] = string.Join(",", list);
                
                return RedirectToAction("ViewInvoice");
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
            }*/
        
    }
}
