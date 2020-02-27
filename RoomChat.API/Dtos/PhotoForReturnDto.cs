using System;

namespace RoomChat.API.Dtos
{
    public class PhotoForReturnDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; } // for Cloudinary usage
    }
}