using Microsoft.AspNetCore.Hosting;
using System;
using System.ComponentModel.DataAnnotations;

namespace RazorHotelDB.Models
{
    public class Hotel
    {
        [Required(ErrorMessage = "HotelNo is required")]
        [Range(1,9999, ErrorMessage = "HotelNo must be a number bestween 1 and 9999")]
        public int HotelNr { get; set; }


        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Name can be a maximum of 30 characters long")]
        public string Navn { get; set; }


        // ^                markerer starten af strengen.
        // [a-zA-Z]         strengen skal starte med et bogstav (stort eller småt).
        // [a-zA-Z0-9 ]*    tillader bogstaver, tal og mellemrum efter første bogstav.
        // \d               kræver mindst ét tal et eller andet sted i strengen.
        // [a-zA-Z0-9 ]*    tillader flere bogstaver, tal og mellemrum efter det obligatoriske tal.
        // $                markerer slutningen af strengen.
        [Required(ErrorMessage = "Address is required")]
        [StringLength(50, ErrorMessage = "Address can be a maximum of 50 characters long")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9 ]*\d[a-zA-Z0-9 ]*$", ErrorMessage = "Address must contain both characters and numbers and no special characters")]
        public string Adresse { get; set; }


        public string? Image { get; set; }

        public Hotel()
        {
            
        }
        public Hotel(int hotelNr, string navn, string adresse, string? image = null)
        {
            HotelNr = hotelNr;
            Navn = navn;
            Adresse = adresse; 
            Image = string.IsNullOrEmpty(image) ? "hotel_img1.jpg" : image;
        }

        public override string ToString()
        {
            return $"{nameof(HotelNr)}: {HotelNr}, {nameof(Navn)}: {Navn}, {nameof(Adresse)}: {Adresse}";
        }
    }
}
