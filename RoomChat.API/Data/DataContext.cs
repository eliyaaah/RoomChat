using RoomChat.API.Models;
using Microsoft.EntityFrameworkCore;

namespace RoomChat.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<Value> Values { get; set; }  //scuffolded table will get this name
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}