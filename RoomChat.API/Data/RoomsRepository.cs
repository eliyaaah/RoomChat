using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoomChat.API.Models;

namespace RoomChat.API.Data
{
    public class RoomsRepository : IRoomsRepository
    {
        private readonly DataContext _context;
        public RoomsRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<Room> GetRoom(int id)
        {
            return await _context.Rooms.Include(r => r.RoomUsers).FirstOrDefaultAsync(r => r.Id == id);
        }
        public void DeleteUserFromRoom(int userId, int roomId)
        {
            _context.Rooms.Find(roomId).RoomUsers.Remove(_context.RoomUser.Find(userId, roomId));
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}