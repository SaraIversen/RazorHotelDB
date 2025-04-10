using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Hotels
{
    public class DeleteHotelModel : PageModel
    {
        private IHotelServiceAsync _hotelServiceAsync;
        public Hotel? Hotel { get; private set; }
        public bool IsDeleted { get; private set; }

        public DeleteHotelModel(IHotelServiceAsync hotelServiceAsync)
        {
            _hotelServiceAsync = hotelServiceAsync;
        }

        public async Task OnGet(int hotelNo)
        {
            try
            {
                Hotel = await _hotelServiceAsync.GetHotelFromIdAsync(hotelNo);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
        public async Task OnPost(int hotelNo)
        {
            try
            {
                Hotel = await _hotelServiceAsync.DeleteHotelAsync(hotelNo);
                IsDeleted = Hotel != null;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
