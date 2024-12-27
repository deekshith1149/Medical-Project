using Medical_Appointment_Management_System.DTOs;
using Medical_Appointment_Management_System.Interfaces;
using Medical_Appointment_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Medical_Appointment_Management_System.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.RegisterUserAsync(userDto.Username, userDto.Password, userDto.Email);
            if (user == null)
                return BadRequest("Username already exists.");

            return Ok(user); // Or return a minimal DTO for security
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userDto)
        {
            var user = await _userService.AuthenticateUserAsync(userDto.Username, userDto.PasswordHash);
            if (user == null) return Unauthorized();

            var token = await _userService.GenerateJwtTokenAsync(userDto.Username);
            if (token == null) return StatusCode(500, "Could not generate token");

            return Ok(new { Token = token });
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}
