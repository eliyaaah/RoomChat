using System.Collections.Generic;

namespace RoomChat.API.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdminId { get; set; }
        public ICollection<RoomUser> RoomUsers { get; set; }
        public ICollection<MessageInRoom> MessagesInRoom { get; set; }
    }
}