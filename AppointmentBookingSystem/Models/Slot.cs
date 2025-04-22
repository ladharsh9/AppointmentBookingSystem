using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBookingSystem.Models
{
    [Index(nameof(StartTime), IsUnique = true)]
    public class Slot
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }

        public DateTime StartTime { get; set; }

       
        public DateTime EndTime { get; set; }

        public bool IsBooked { get; set; } = false;
    }

}
