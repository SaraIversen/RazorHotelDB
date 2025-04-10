using RazorHotelDB.Enums;
using System.ComponentModel.DataAnnotations;

namespace RazorHotelDB.Models
{
    public class Room
    {
        [Required(ErrorMessage = "RoomNo is required")]
        [Range(1, 9999, ErrorMessage = "RoomNo must be a number bestween 1 and 9999")]
        public int RoomNr { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [EnumDataType(typeof(RoomType), ErrorMessage = "Type must be S, D, or F")]
        public RoomType Types { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 9999999, ErrorMessage = "Price must be a number bestween 1 and 9999999")]
        public double Pris { get; set; }

        public int HotelNr { get; set; }

        public Room()
        {

        }
        public Room(int nr, string types, double pris)
        {
            RoomNr = nr;
            Types = Enum.TryParse(types, out RoomType roomType) ? roomType : RoomType.S;
            Pris = pris;
        }

        public Room(int nr, string types, double pris, int hotelNr) : this(nr, types, pris)
        {
            HotelNr = hotelNr;
        }

        public override string ToString()
        {
            return $"Room = {RoomNr}, Types = {Types}, Pris = {Pris}";
        }
    }
}
