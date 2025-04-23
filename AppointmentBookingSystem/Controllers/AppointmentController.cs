using AppointmentBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBookingSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult Book()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var slots = _context.Slots.ToList(); 
            return View(slots);
        }
        [HttpPost]
        public ActionResult Book(int slotId)
        {
            //int userId = (int)Session["UserId"];
            //    int? userId = HttpContext.Session.GetInt32("UserId");

            //    var slot = _context.Slots.Find(slotId);
            //    if (slot != null && !slot.IsBooked)
            //    {
            //        slot.IsBooked = true;
            //        _context.Appointments.Add(new Appointment { UserId = userId, SlotId = slotId });
            //        _context.SaveChanges();
            //        ViewBag.Message = "Appointment Booked!";
            //    }
            //    return RedirectToAction("Book");

            //}
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            //var slot = _context.Slots.Find(slotId);

            //if (slot != null && !slot.IsBooked)
            //{
            //    slot.IsBooked = true;
            //    _context.Appointments.Add(new Appointment
            //    {
            //        UserId = userId.Value,
            //        SlotId = slotId
            //    });

            //    _context.SaveChanges();
            //    ViewBag.Message = "Appointment Booked!";
            //    TempData["Message"] = "Appointment Booked successfully!";
            //}
           //return RedirectToAction("Book");

            var slot = _context.Slots.FirstOrDefault(s => s.Id == slotId);
            if (slot == null || slot.IsBooked)
            {
                TempData["Message"] = "Slot is already booked!";
                return RedirectToAction("Book");
            }
            
            bool alreadyBooked = _context.Appointments.Any(a => a.SlotId == slotId);
            if (alreadyBooked)
            {
                TempData["Message"] = "This slot has already been booked!";
                return RedirectToAction("Book");
            }

          
            var appointment = new Appointment
            {
                SlotId = slotId,
                UserId = userId.Value
            };

           
            slot.IsBooked = true;
            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            TempData["Message"] = "Appointment booked successfully!";
            return RedirectToAction("Book");
        
          
        }
        public ActionResult MyAppointments()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId"); // Get UserId from session
            var appointments = _context.Appointments
                .Where(a => a.UserId == userId)
                .Include(a => a.Slot) // Include related Slot data
                .ToList();

            return View(appointments);
        }

        public IActionResult AddSlot()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "True")
                return Unauthorized();

            return View();
        }
        [HttpPost]
        public IActionResult AddSlot(Slot slot)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin != "True")
                return Unauthorized();

            if (ModelState.IsValid)
            {
                var existingSlot = _context.Slots
                .FirstOrDefault(s => s.StartTime == slot.StartTime);

                if (existingSlot != null)
                {
                    ViewBag.Error = "A slot with this time already exists.";
                    return View();
                }

                //  1-hour duration is set 
                slot.EndTime = slot.StartTime.AddHours(1);
                slot.IsBooked = false;


                _context.Slots.Add(slot);
                _context.SaveChanges();
                ViewBag.Message = "Slot added successfully!";
            }

            return View();
        }
        public IActionResult ManageSlots()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
                return Unauthorized();

            var slots = _context.Slots
                .Select(s => new ManageDTO//new
                {
                    Id = s.Id,
                    DoctorName = s.DoctorName,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsBooked = s.IsBooked,
                    //UserName = _context.Appointments
                    //    .Where(a => a.SlotId == s.Id)
                    //    .Select(a => a.User.UserName)
                    //    .FirstOrDefault()//till here
                    UserName = s.IsBooked
                        ? _context.Appointments
                            .Where(a => a.SlotId == s.Id)
                            .Select(a => a.User.UserName)
                            .FirstOrDefault()
                        : null,
                }).ToList();
            return View(slots);
        }
        public IActionResult EditSlot(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
                return Unauthorized();

            var slot = _context.Slots.Find(id);
            if (slot == null)
                return NotFound();

            return View(slot);
        }

        [HttpPost]
        public IActionResult EditSlot(Slot updatedSlot)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
                return Unauthorized();

            if (ModelState.IsValid)
            {
               
                var conflictingSlot = _context.Slots
                    .FirstOrDefault(s => s.StartTime == updatedSlot.StartTime && s.Id != updatedSlot.Id);

                if (conflictingSlot != null)
                {
                    ViewBag.Error = "A slot with this time already exists.";
                    return View(updatedSlot);
                }

                updatedSlot.EndTime = updatedSlot.StartTime.AddHours(1);
                
                if (!updatedSlot.IsBooked)
                {
                    var existingAppointment = _context.Appointments
                        .FirstOrDefault(a => a.SlotId == updatedSlot.Id);

                    if (existingAppointment != null)
                    {
                        _context.Appointments.Remove(existingAppointment);
                    }
                }



                _context.Slots.Update(updatedSlot);
                _context.SaveChanges();
                return RedirectToAction("ManageSlots");
            }
            return View(updatedSlot);
        }
        public IActionResult DeleteSlot(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
                return Unauthorized();

            var slot = _context.Slots.Find(id);
            if (slot == null)
                return NotFound();

            _context.Slots.Remove(slot);
            _context.SaveChanges();
            return RedirectToAction("ManageSlots");
        }

        [HttpPost]
        public IActionResult Cancel(int appointmentId)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");

            var appointment = _context.Appointments
                .Include(a => a.Slot)
                .FirstOrDefault(a => a.Id == appointmentId && a.UserId == userId);
            if (appointment == null)
            {
                return NotFound();
            }
            
            if (appointment.Slot != null)
            {
                appointment.Slot.IsBooked = false;
            }

            
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();

         
            return RedirectToAction("MyAppointments");
        }


    }
    }
