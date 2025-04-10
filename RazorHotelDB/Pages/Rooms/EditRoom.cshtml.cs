using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorHotelDB.Enums;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;

namespace RazorHotelDB.Pages.Rooms
{
    public class EditRoomModel : PageModel
    {
        private IRoomService _roomService;

        public bool IsUpdated { get; set; }
        [BindProperty] public Room Room { get; set; }
        [BindProperty] public int OldRoomNo { get; set; }


        public List<SelectListItem> RoomTypeSelectList { get; set; }


        public EditRoomModel(IRoomService roomService)
        {
            _roomService = roomService;

            CreateRoomTypeSelectList();
        }

        private void CreateRoomTypeSelectList()
        {
            RoomTypeSelectList = new List<SelectListItem>();
            int i = 0;
            foreach (RoomType roomType in Enum.GetValues(typeof(RoomType)))
            {
                SelectListItem selectListItem = new SelectListItem($"{roomType}", i.ToString());
                RoomTypeSelectList.Add(selectListItem);
                i += 1;
            }
        }

        public async Task OnGet(int roomNo, int hotelNo)
        {
            try
            {
                OldRoomNo = roomNo;
                Room = await _roomService.GetRoomFromIdAsync(roomNo, hotelNo);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        public async Task OnPost(int oldRoomNo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IsUpdated = await _roomService.UpdateRoomAsync(Room, oldRoomNo, Room.HotelNr);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
