using System.Threading.Tasks;
using RoomChat.API.Models;

namespace RoomChat.API.Data
{
    public interface IRoomsRepository
    {
        void Add<T>(T entity) where T:class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();   // will return true for saved changes; false for nothing to save
        Task<Room> GetRoom(int id);
        void DeleteUserFromRoom(int userId, int roomId);
    }
}