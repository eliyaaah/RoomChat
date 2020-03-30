using System.Collections.Generic;
using System.Threading.Tasks;
using RoomChat.API.Helpers;
using RoomChat.API.Models;

namespace RoomChat.API.Data
{
    public interface IUserRepository
    {
        void Add<T>(T entity) where T:class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();   // will return true for saved changes; false for nothing to save
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<Connection> GetConnectionRequest(int userId, int recipientId);
        Task<List<string>> GetCompanyList();
        Task<List<string>> GetLocationList();
    }
}