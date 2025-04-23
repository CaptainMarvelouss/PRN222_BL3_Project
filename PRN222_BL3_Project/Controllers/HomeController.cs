using Microsoft.AspNetCore.Mvc;
using PRN222_BL3_Project.Models;
using System.Diagnostics;
using Repositories;

namespace PRN222_BL3_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFootballFieldRepository _fieldRepo;
        //private readonly IEmailService _emailService;

        //public HomeController(ILogger<HomeController> logger, IFieldRepository fieldRepo, IEmailService emailService)
        public HomeController(ILogger<HomeController> logger, IFootballFieldRepository fieldRepo)
        {
            _logger = logger;
            _fieldRepo = fieldRepo;
            //_emailService = emailService;
        }
        public async Task<IActionResult> Index()
        {
            var fields = _fieldRepo.GetFootballFields();
            ViewBag.ShowWelcomeModal = !Request.Cookies.ContainsKey("WelcomeModalSeen");

            return View(fields); // send to Index.cshtml
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpPost]
        //public async Task<IActionResult> Subscribe(string email)
        //{
        //    if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
        //    {
        //        TempData["ErrorMessage"] = "Please enter a valid email address";
        //        return RedirectToAction("Index");
        //    }

        //    try
        //    {
        //        await _emailService.SubscribeToNewsletterAsync(email);
        //        TempData["SuccessMessage"] = "Thank you for subscribing!";
        //    }
        //    catch
        //    {
        //        TempData["ErrorMessage"] = "There was an error processing your subscription.";
        //    }

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult CloseWelcomeModal()
        {
            Response.Cookies.Append("WelcomeModalSeen", "true", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30)
            });

            return Ok();
        }

        //private bool IsValidEmail(string email)
        //{
        //    try
        //    {
        //        var addr = new System.Net.Mail.MailAddress(email);
        //        return addr.Address == email;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}