using System.Security.Cryptography;
using System.Text;
using Medical_Appointment_Management_System.Interfaces.Interfaces;
using Medical_Appointment_Management_System.Interfaces;
using Medical_Appointment_Management_System.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Medical_Appointment_Management_System.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string?> GenerateJwtTokenAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) return null;

            var key = _configuration["Jwt:Key"] ?? "ThisIsASecretKeyForJWT";
            var issuer = _configuration["Jwt:Issuer"] ?? "YourAppIssuer";

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", user.Role)
        };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<User?> RegisterUserAsync(string username, string password, string email)
        {
            if (await _userRepository.GetUserByUsernameAsync(username) != null)
                return null; // Username already exists

            var hashedPassword = HashPassword(password);
            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Email = email
            };

            await _userRepository.CreateUserAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }


        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<User?> GetUserByIdAsync(Guid id) => await _userRepository.GetUserByIdAsync(id);

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hashedPassword) =>
            HashPassword(password) == hashedPassword;
    }
}




