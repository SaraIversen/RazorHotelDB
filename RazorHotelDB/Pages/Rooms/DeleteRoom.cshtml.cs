using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Rooms
{
    public class DeleteRoomModel : PageModel
    {
        private IRoomService _roomService;

        public int HotelNr { get; set; }
        public Room? Room { get; private set; }
        public bool IsDeleted { get; private set; }

        public DeleteRoomModel(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task OnGet(int roomNo, int hotelNo)
        {
            try
            {
                HotelNr = hotelNo;
                Room = await _roomService.GetRoomFromIdAsync(roomNo, hotelNo);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
        public async Task OnPost(int roomNo, int hotelNo)
        {
            try
            {
                HotelNr = hotelNo;
                Room = await _roomService.DeleteRoomAsync(roomNo, hotelNo);
                IsDeleted = Room != null;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
