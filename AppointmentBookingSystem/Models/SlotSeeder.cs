namespace AppointmentBookingSystem.Models
{
    public static class SlotSeeder
    {
        public static void SeedSlots(AppDbContext context)
        {
            if (!context.Slots.Any())
            {
                context.Slots.AddRange(new List<Slot>
            {
                new Slot { StartTime = DateTime.Today.AddHours(13), EndTime = DateTime.Today.AddHours(14),IsBooked = false  },
                new Slot { StartTime = DateTime.Today.AddHours(14), EndTime = DateTime.Today.AddHours(15), IsBooked = false  },
                new Slot { StartTime = DateTime.Today.AddHours(15), EndTime = DateTime.Today.AddHours(16),IsBooked = false  }
            });

                context.SaveChanges();
            }
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    Password = "admin123",
                    IsAdmin = true
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}
