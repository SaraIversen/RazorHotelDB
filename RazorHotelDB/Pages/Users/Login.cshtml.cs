using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Users
{
    public class LoginModel : PageModel
    {
        private IUserService _userService;

        [BindProperty] public string UserName { get; set; }
        [BindProperty] public string Password { get; set; }

        public string Message { get; set; }


        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public void OnGetLogout()
        {
            HttpContext.Session.Remove("UserName");
        }

        public async Task<IActionResult> OnPost()
        {
            User? loginUser = await _userService.VerifyUser(UserName, Password);
            if (loginUser != null)
            {
                HttpContext.Session.SetString("UserName", loginUser.UserName);
                return RedirectToPage("../Hotels/GetAllHotels");
            }
            else
            {
                Message = "Invalid username or password";
                UserName = "";
                Password = "";
                return Page();
            }
        }
    }
}
