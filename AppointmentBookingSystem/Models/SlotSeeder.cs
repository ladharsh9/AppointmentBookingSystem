namespace AppointmentBookingSystem.Models
{
    public static class SlotSeeder
    {
        public static void SeedSlots(AppDbContext context)
        {
           
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    Password = "admin@123",
                    IsAdmin = true
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}
