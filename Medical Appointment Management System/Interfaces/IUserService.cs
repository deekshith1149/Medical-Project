using Medical_Appointment_Management_System.Models;


namespace Medical_Appointment_Management_System.Interfaces
{
    public interface IUserService
    {
        Task<User?> RegisterUserAsync(string username, string password, string email);
        Task<User?> AuthenticateUserAsync(string username, string password);
        Task<User?> GetUserByIdAsync(Guid id);

        Task<string?> GenerateJwtTokenAsync(string username);

    }
}





