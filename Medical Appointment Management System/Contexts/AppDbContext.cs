using Medical_Appointment_Management_System.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Medical_Appointment_Management_System.Contexts

{
    public class AppDbContext : DbContext
{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}

