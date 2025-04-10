using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Hotels
{
    public class CreateHotelModel : PageModel
    {
        private IHotelServiceAsync _hotelService;
        private IWebHostEnvironment _webHostEnvironment;

        public bool IsCreated { get; set; }
        [BindProperty] public Hotel NewHotel { get; set; }
        [BindProperty] public IFormFile? Photo { get; set; }


        public CreateHotelModel(IHotelServiceAsync hotelServiceAsync, IWebHostEnvironment webHostEnvironment)
        {
            _hotelService = hotelServiceAsync;
            _webHostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {
            NewHotel = new();
        }

        public async Task OnPostAsync()
        {
            try
            {
                if (Photo != null) // Only process image upload if there's a file
                {
                    // Not working somehow?
/*                    if (!string.IsNullOrEmpty(NewHotel.Image)) // If the hotel already has an image connected.
                    {
                        string filepath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "hotelImages", NewHotel.Image!);
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath); // Then make sure to delete the old image.
                        }
                    }*/
                    NewHotel.Image = ProcessUploadedFile(); // Upload the new image and save it in the chosen roots/images folder.
                }

                if (ModelState.IsValid)
                {
                    IsCreated = await _hotelService.CreateHotelAsync(NewHotel);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;
            if (Photo != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                using (var fileStream = new FileStream(Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath, "images/hotelImages"), uniqueFileName), FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
