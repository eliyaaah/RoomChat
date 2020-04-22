using System;

namespace RoomChat.API.Models
{
    public class MessageInRoom
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public bool IsFile { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime? MessageSent { get; set; }
        public bool Deleted { get; set; }
    }
}