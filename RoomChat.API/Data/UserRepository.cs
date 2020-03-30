using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoomChat.API.Helpers;
using RoomChat.API.Models;

namespace RoomChat.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity); // only will be saved in memory (not db) until SaveAll() is executed
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity); // only will be saved in memory (not) until SaveAll() is executed
        }

        public async Task<Connection> GetConnectionRequest(int userId, int recipientId)
        {
            return await _context.Connections.FirstOrDefaultAsync(u => u.UserId1 == userId && u.UserId2 == recipientId);
        }

        public Task<Photo> GetMainPhotoForUser(int userId)
        {
            return _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            if (!string.IsNullOrEmpty(userParams.Company))
            {
                users = users.Where(u => u.Company == userParams.Company);
            }

            if (!string.IsNullOrEmpty(userParams.Location))
            {
                users = users.Where(u => u.Location == userParams.Location);
            }

            if(userParams.Connections)
            {
                var userConnections = await GetUserConnections(userParams.UserId, userParams.ConnectionRequests);
                users = users.Where(u => userConnections.Contains(u.Id));
            }

            if(userParams.ConnectionRequests)
            {
                var userConnectionRequests = await GetUserConnections(userParams.UserId, userParams.ConnectionRequests);                
                users = users.Where(u => userConnectionRequests.Contains(u.Id));
            }

            if(!userParams.Connections && !userParams.ConnectionRequests)
            {
                var userConnections = await GetUserConnections(userParams.UserId, userParams.ConnectionRequests);
                users = users.Where(u => !userConnections.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch(userParams.OrderBy)
                {
                    case "created": 
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users =users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserConnections(int id, bool requestsReceived)
        {
            var user = await _context.Users.Include(x => x.ConnectionRequestsReceived).Include(x => x.ConnectionRequestsSent)
                .FirstOrDefaultAsync(u => u.Id == id);


            if (requestsReceived)
            {
                var userRequestsSent = user.ConnectionRequestsSent.Where(u => u.UserId1 == id).Select(i => i.UserId2);             
                return user.ConnectionRequestsReceived.Where(u => u.UserId2 == id)
                    .Where(u => !userRequestsSent.Any(u2 => u2 == u.UserId1)).Select(i => i.UserId1);
            }
            else
            {
                var userRequestsReceived = user.ConnectionRequestsReceived.Where(u => u.UserId2 == id).Select(i => i.UserId1);
                return user.ConnectionRequestsSent.Where(u => u.UserId1 == id)
                    .Where(u => userRequestsReceived.Any(u2 => u2 == u.UserId2)).Select(i => i.UserId2);
            }
        }

        public async Task<List<string>> GetCompanyList()
        {
            return await _context.Users.Select(s => s.Company).Distinct().ToListAsync();
        }

        public async Task<List<string>> GetLocationList()
        {
            return await _context.Users.Select(s => s.Location).Distinct().ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}