namespace AppointmentBookingSystem.Models
{
    public class SlotwithUser
    {
        public int SlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsBooked { get; set; } = false;
        public string? UserName { get; set; }
    }
}
