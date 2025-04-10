using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorHotelDB.Enums;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;
using System.Runtime.CompilerServices;

namespace RazorHotelDB.Pages.Rooms
{
    public class CreateRoomModel : PageModel
    {
        private IRoomService _roomService;
        private IHotelServiceAsync _hotelService;

        public bool IsCreated { get; set; }
        [BindProperty] public Room NewRoom { get; set; }
        public Hotel Hotel { get; set; }


        public List<SelectListItem> RoomTypeSelectList { get; set; }


        public CreateRoomModel(IRoomService roomService, IHotelServiceAsync hotelService)
        {
            _roomService = roomService;
            _hotelService = hotelService;

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

        public async Task OnGetAsync(int hotelNo)
        {
            try
            {
                Hotel = await _hotelService.GetHotelFromIdAsync(hotelNo);
                NewRoom = new();
                NewRoom.HotelNr = hotelNo;
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
                Hotel = await _hotelService.GetHotelFromIdAsync(NewRoom.HotelNr);

                if (ModelState.IsValid)
                {
                    IsCreated = await _roomService.CreateRoomAsync(NewRoom.HotelNr, NewRoom);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
