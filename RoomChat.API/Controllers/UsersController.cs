using System;
using System.Collections.Generic;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;
            
            // if (string.IsNullOrEmpty(userParams.Company))
            // {
            //     userParams.Company = userFromRepo.Company;
            // }

            // if (string.IsNullOrEmpty(userParams.Location))
            // {
            //     userParams.Location = userFromRepo.Location;
            // }

            var users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, 
                users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            // checking if user id is matching their token
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPost("{id}/connect/{recipientId}")]
        public async Task<IActionResult> ConnectWithUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var connection = await _repo.GetConnectionRequest(id, recipientId);

            if (connection != null)
                return BadRequest("You have already sent the connection request to this user");
            
            if (await _repo.GetUser(recipientId) == null)
                return NotFound();
            
            connection = new Connection
            {
                UserId1 = id,
                UserId2 = recipientId
            };

            _repo.Add<Connection>(connection);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to send connection request");
        }

        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanyList()
        {
            var companyList = await _repo.GetCompanyList();
            return Ok(companyList);
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetLocationList()
        {
            var locationList = await _repo.GetLocationList();
            return Ok(locationList);
        }
    }
}