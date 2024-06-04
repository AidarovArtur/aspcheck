using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourNamespace.Repositories;
using AutoMapper;
using task1.DTOs;
using task1.Models;
using task1.Repositories;

namespace task1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            user.ID = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.PasswordHash = "Random hash is here by the way:)";
            user.Role = request.Role ?? "User";

            await _userRepository.CreateUserAsync(user);
            return Ok(new ApiResponse { Success = true, Message = "User created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userResponses = _mapper.Map<IEnumerable<UserResponse>>(users);
            return Ok(userResponses);
        }

        [HttpGet("me/{id}")]
        public async Task<IActionResult> GetCurrentUser(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var userResponse = _mapper.Map<UserResponse>(user);
            return Ok(userResponse);
        }

        [HttpPut("me/{id}")]
        public async Task<IActionResult> UpdateCurrentUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            _mapper.Map(request, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(user); // change it DB
            return Ok(new ApiResponse { Success = true, Message = "User updated successfully" });
        }

        [HttpDelete("me/{id}")]
        public async Task<IActionResult> DeleteCurrentUser(Guid id)
        {
            var success = await _userRepository.DeleteUserAsync(id);
            if (!success)
                return NotFound(new ApiResponse { Success = false, Message = "User not found" });

            return Ok(new ApiResponse { Success = true, Message = "User deleted successfully" });
        }
    }
}