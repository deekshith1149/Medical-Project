using Medical_Appointment_Management_System.Contexts;
using Medical_Appointment_Management_System.Interfaces.Interfaces;
using Medical_Appointment_Management_System.Models;
using Microsoft.EntityFrameworkCore;


namespace Medical_Appointment_Management_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(Guid id) => await _context.Users.FindAsync(id);

        public async Task<User?> GetUserByUsernameAsync(string username) =>
            await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

        public async Task CreateUserAsync(User user) => await _context.Users.AddAsync(user);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
