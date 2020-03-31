using System;
using System.Collections.Generic;

namespace RoomChat.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string DisplayName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Connection> ConnectionRequestsSent { get; set; }
        public ICollection<Connection> ConnectionRequestsReceived { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
    }
}