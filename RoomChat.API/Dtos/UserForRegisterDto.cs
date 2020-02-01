using System.ComponentModel.DataAnnotations;

namespace RoomChat.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "You must specify password between 8 and 16 characters")]
        public string Password { get; set; }
    }
}