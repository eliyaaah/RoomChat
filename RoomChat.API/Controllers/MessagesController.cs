using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomChat.API.Data;
using RoomChat.API.Helpers;

namespace RoomChat.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // [HttpGet("{id}", Name = "GetMessage")]
        // public async 
    }
}