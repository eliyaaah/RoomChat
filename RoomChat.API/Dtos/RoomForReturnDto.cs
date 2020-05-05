using System;
using System.Collections.Generic;
using RoomChat.API.Models;

namespace RoomChat.API.Dtos
{
    public class RoomForReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdminId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}