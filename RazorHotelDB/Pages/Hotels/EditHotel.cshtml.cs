using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;

namespace RazorHotelDB.Pages.Hotels
{
    public class EditHotelModel : PageModel
    {
        private IHotelServiceAsync _hotelServiceAsync;
        private IWebHostEnvironment _webHostEnvironment;

        public bool IsUpdated { get; set; }
        [BindProperty] public Hotel Hotel { get; set; }

        public bool NewPhotoUploaded { get; set; }
        [BindProperty] public IFormFile? Photo { get; set; }


        public EditHotelModel(IHotelServiceAsync hotelServiceAsync, IWebHostEnvironment webHostEnvironment)
        {
            _hotelServiceAsync = hotelServiceAsync;
            _webHostEnvironment = webHostEnvironment;
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

        public async Task OnPost()
        {
            try
            {
                if (Photo != null && !string.IsNullOrEmpty(Hotel.Image))
                {
                    ProcessUploadedFile();
                }

                if (ModelState.IsValid)
                {
                    IsUpdated = await _hotelServiceAsync.UpdateHotelAsync(Hotel, Hotel.HotelNr);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        private void ProcessUploadedFile()
        {
            using (var fileStream = new FileStream(Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath, "images/hotelImages"), Hotel.Image), FileMode.Create))
            {
                Photo.CopyTo(fileStream);
            }
        }
    }
}
