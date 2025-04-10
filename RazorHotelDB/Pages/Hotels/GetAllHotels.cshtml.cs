using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Enums;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;

namespace RazorHotelDB.Pages.Hotels
{
    public class GetAllHotelsModel : PageModel
    {
        private IHotelServiceAsync _hotelService;

        public string? CurrentUser { get; private set; }

        [BindProperty(SupportsGet = true)] public string FilterCriteria { get; set; }
        [BindProperty(SupportsGet = true)] public string SortOrder { get; set; } = "HotelNr"; // Set default to "HotelNr"
        [BindProperty(SupportsGet = true)] public string SortOrderAscDesc { get; set; }

        public List<Hotel> Hotels { get; set; }


        public GetAllHotelsModel(IHotelServiceAsync hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task OnGetAsync()
        {
            try
            {
                CurrentUser = HttpContext.Session.GetString("UserName");

                if (string.IsNullOrEmpty(FilterCriteria))
                {
                    Hotels = await _hotelService.GetAllHotelAsync();
                }
                else
                {
                    Hotels = await _hotelService.GetHotelsByNameAsync(FilterCriteria);
                }

                FilterHotels();
            }
            catch(Exception ex)
            {
                Hotels = new List<Hotel>();
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        private void FilterHotels()
        {
            if (SortOrder == "Navn")
                Hotels.Sort((n1, n2) => n1.Navn.CompareTo(n2.Navn));
            if (SortOrder == "Adresse")
                Hotels.Sort((a1, a2) => a1.Adresse.CompareTo(a2.Adresse));
            if (SortOrderAscDesc == "Descending")
                Hotels.Reverse();
        }
    }
}
