using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;

namespace RazorHotelDB.Pages.Rooms
{
    public class GetAllRoomsModel : PageModel
    {
        private IRoomService _roomService;

        public List<Room> Rooms { get; set; }

        public GetAllRoomsModel(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Rooms = await _roomService.GetAllRoomAsync();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
