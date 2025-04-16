using System.ComponentModel.DataAnnotations;

namespace AppointmentBookingSystem.Models
{
    public class Slot
    {
        public int Id { get; set; }

       
        public DateTime StartTime { get; set; }

       
        public DateTime EndTime { get; set; }

        public bool IsBooked { get; set; } = false;
    }

}
