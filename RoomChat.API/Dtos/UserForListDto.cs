using System;

namespace RoomChat.API.Dtos
{
    public class UserForListDto
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastActive { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string PhotoUrl { get; set; }
    }
}