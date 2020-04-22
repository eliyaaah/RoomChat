namespace RoomChat.API.Models
{
    public class RoomUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}