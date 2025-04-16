namespace AppointmentBookingSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int SlotId { get; set; }
        public virtual Slot Slot { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;
    }

}
