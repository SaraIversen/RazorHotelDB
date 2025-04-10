using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorHotelDB.Pages.Users
{
    public class LogoutModel : PageModel
    {
        public string? CurrentUser { get; private set; }

        public IActionResult OnGet()
        {
            #region ValidateUser
            CurrentUser = HttpContext.Session.GetString("UserName");
            if (CurrentUser == null)
            {
                return RedirectToPage("Login");
            }
            else return Page();
            #endregion
        }
    }
}
