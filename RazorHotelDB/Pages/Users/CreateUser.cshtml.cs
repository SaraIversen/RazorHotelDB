using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;

namespace RazorHotelDB.Pages.Users
{
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;

        public bool IsCreated { get; set; }
        [BindProperty] public User User { get; set; }

        public CreateUserModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
            User = new();
        }

        public async Task OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IsCreated = await _userService.CreateUserAsync(User);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
