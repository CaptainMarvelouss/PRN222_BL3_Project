using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN222_BL3_Project.Views.HomePage
{
    public class IndexModel : PageModel
    {
        private readonly IFootballFieldRepository _fieldRepo;
        //private readonly IEmailRepository _emailService;

        public List<Field> Fields { get; set; }
        public bool ShowWelcomeModal { get; set; }

        //public IndexModel(IFootballFieldRepository fieldRepo, IEmailRepository   emailService)
        public IndexModel(IFootballFieldRepository fieldRepo)
        {
            _fieldRepo = fieldRepo;
            //_emailService = emailService;
        }

        public void OnGet(string culture = "en")
        {
            // Set culture for internationalization if needed
            //if (!string.IsNullOrEmpty(culture))
            //{
            //    // Culture handling logic would go here
            //}

            // Get locations from service
            Fields = _fieldRepo.GetFootballFields();

            // Check if we should show the welcome modal. Could use cookies or session state to determine this
            ShowWelcomeModal = !Request.Cookies.ContainsKey("WelcomeModalSeen");
        }

        //public async Task<IActionResult> OnPostSubscribeAsync(string email)
        //{
        //    if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
        //    {
        //        TempData["ErrorMessage"] = "Please enter a valid email address";
        //        return RedirectToPage();
        //    }

        //    try
        //    {
        //        await _emailService.SubscribeToNewsletterAsync(email);
        //        TempData["SuccessMessage"] = "Thank you for subscribing!";
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "There was an error processing your subscription. Please try again.";
        //        // Log the exception
        //    }

        //    return RedirectToPage();
        //}

        public IActionResult OnGetCloseWelcomeModal()
        {
            // Set a cookie to remember that the user has seen the welcome modal
            Response.Cookies.Append("WelcomeModalSeen", "true", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30)
            });

            return new EmptyResult();
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
