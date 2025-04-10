using System.ComponentModel.DataAnnotations;

namespace RazorHotelDB.Models
{
    public class User
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, 9999, ErrorMessage = "Id must be a number bestween 1 and 9999")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Name can be a maximum of 30 characters long")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Password can be a maximum of 30 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Email can be a maximum of 50 characters long")]
        public string Email { get; set; }

        public User()
        {

        }
        public User(int id, string userName, string password, string email)
        {
            Id = id;
            UserName = userName;
            Password = password;
            Email = email;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(UserName)}: {UserName}, {nameof(Password)}: {Password}, {nameof(Email)}: {Email}";
        }
    }
}
