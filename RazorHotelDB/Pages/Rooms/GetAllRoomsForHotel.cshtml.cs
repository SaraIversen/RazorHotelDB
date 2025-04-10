using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using RazorHotelDB.Enums;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;

namespace RazorHotelDB.Pages.Rooms
{
    public class GetAllRoomsForHotelModel : PageModel
    {
        private IRoomService _roomService;
        private IHotelServiceAsync _hotelService;

        public string? CurrentUser { get; private set; }


        [BindProperty(SupportsGet = true)] public string FilterRoomType { get; set; }
        [BindProperty(SupportsGet = true)] public string SortOrderAscDesc { get; set; }
        [BindProperty(SupportsGet = true)] public int FilterMaxPrice { get; set; }


        public Hotel Hotel { get; private set; }
        public List<Room> Rooms { get; set; }


        public GetAllRoomsForHotelModel(IRoomService roomService, IHotelServiceAsync hotelService)
        {
            _roomService = roomService;
            _hotelService = hotelService;
        }

        public async Task OnGetAsync(int hotelNo)
        {
            try
            {
                CurrentUser = HttpContext.Session.GetString("UserName");

                Hotel = await _hotelService.GetHotelFromIdAsync(hotelNo);
                Rooms = await _roomService.GetAllRoomForHotelAsync(hotelNo);

                FilterRooms();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }

        private void FilterRooms()
        {
            if (FilterMaxPrice > 0)
            {
                Rooms = Rooms.FindAll(r => r.Pris <= FilterMaxPrice);
            }

            if (!FilterRoomType.IsNullOrEmpty() && FilterRoomType != "All")
            {
                RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType), FilterRoomType);
                Rooms = Rooms.FindAll(r => r.Types == roomType);
            }

            if (SortOrderAscDesc == "Descending")
            {
                Rooms.Reverse();
            }
        }
    }
}
