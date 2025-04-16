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
            //if (Session["UserId"] == null)
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var slots = _context.Slots.Where(s => !s.IsBooked).ToList();
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

            var slot = _context.Slots.Find(slotId);
            if (slot != null && !slot.IsBooked)
            {
                slot.IsBooked = true;

                _context.Appointments.Add(new Appointment
                {
                    UserId = userId.Value,
                    SlotId = slotId
                });

                _context.SaveChanges();
                ViewBag.Message = "Appointment Booked!";
                TempData["Message"] = "Appointment Booked successfully!";
            }

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

            var slots = _context.Slots.ToList();
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



    }
}
