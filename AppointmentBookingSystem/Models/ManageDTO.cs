namespace AppointmentBookingSystem.Models
{
    public class ManageDTO
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBooked { get; set; }
        public string? UserName { get; set; }
    }
}
