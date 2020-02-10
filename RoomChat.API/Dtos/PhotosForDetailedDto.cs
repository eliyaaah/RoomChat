using System;

namespace RoomChat.API.Dtos
{
    public class PhotosForDetailedDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public DateTime DateAdded { get; set; }
    }
}