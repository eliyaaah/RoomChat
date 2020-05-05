using System;
using System.Collections.Generic;
using RoomChat.API.Models;

namespace RoomChat.API.Dtos
{
    public class RoomForCreationDto{
        public RoomForCreationDto()
        {
            DateCreated = DateTime.Now;
        }

        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}