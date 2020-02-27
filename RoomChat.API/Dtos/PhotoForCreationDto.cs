using System;
using Microsoft.AspNetCore.Http;

namespace RoomChat.API.Dtos
{
    public class PhotoForCreationDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public string PublicId { get; set; }
        public DateTime DateAdded { get; set; }

        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}