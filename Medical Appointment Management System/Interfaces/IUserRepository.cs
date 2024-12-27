using Medical_Appointment_Management_System.Models;

namespace Medical_Appointment_Management_System.Interfaces.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}




