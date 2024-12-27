namespace Medical_Appointment_Management_System.Models


{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Patient"; // Can be Patient or Doctor
        public string Email { get; set; } = string.Empty;
    }
}

