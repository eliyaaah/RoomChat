using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomChat.API.Data;
using RoomChat.API.Dtos;
using RoomChat.API.Helpers;
using RoomChat.API.Models;

namespace RoomChat.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsRepository _repo;
        private readonly IMapper _mapper;

        public RoomsController(IRoomsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomForCreationDto roomForCreationDto)
        {   
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var room = _mapper.Map<Room>(roomForCreationDto);
            room.AdminId = loggedInUserId;

            _repo.Add(room);

            if (await _repo.SaveAll())
            {
                var roomToReturn = _mapper.Map<RoomForReturnDto>(room);
                
                var roomUser = new RoomUser {
                    RoomId = roomToReturn.Id,
                    UserId = loggedInUserId
                };

                _repo.Add<RoomUser>(roomUser);
                
                if (await _repo.SaveAll())
                    return CreatedAtRoute("GetRoom", new {id = roomToReturn.Id}, roomToReturn);
            }
            
            throw new Exception("Creating room failed on save");
        }

        [HttpGet("{id}", Name = "GetRoom")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var loggedInUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var roomFromRepo = await _repo.GetRoom(id);

            if (roomFromRepo == null)
                return NotFound();
            
            var roomForReturn = _mapper.Map<RoomForReturnDto>(roomFromRepo);

            if ((roomFromRepo.RoomUsers.Any(ru => ru.UserId == loggedInUser)) && (roomFromRepo.AdminId != loggedInUser))
                return Unauthorized();
            
            return Ok(roomForReturn);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var loggedInUse = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var roomFromRepo = await _repo.GetRoom(id);

            if (roomFromRepo.AdminId != loggedInUse)
                return Unauthorized();

            _repo.Delete(roomFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Error deleting the room");
        }

        [HttpPost("{roomId}/users")]
        public async Task<IActionResult> AddUserToRoom(int roomId, int userId)
        {
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var roomFromRepo = await _repo.GetRoom(roomId);

            if (roomFromRepo.AdminId != loggedInUserId)
                return Unauthorized();

            var roomUser = new RoomUser {
                RoomId = roomId,
                UserId = userId
            };

            _repo.Add<RoomUser>(roomUser);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to add user to the room");
        }
    }
}