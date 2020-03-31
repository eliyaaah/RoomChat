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
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Connection>()
                .HasKey(k => new { k.UserId1, k.UserId2 });
            
            builder.Entity<Connection>()
                .HasOne(u => u.User1)
                .WithMany(u => u.ConnectionRequestsSent)
                .HasForeignKey(u => u.UserId1)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Connection>()
                .HasOne(u => u.User2)
                .WithMany(u => u.ConnectionRequestsReceived)
                .HasForeignKey(u => u.UserId2)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}