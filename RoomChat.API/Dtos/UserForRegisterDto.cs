using System;
using System.ComponentModel.DataAnnotations;

namespace RoomChat.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "You must specify password between 8 and 16 characters")]
        public string Password { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Company { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}