using AppointmentBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace AppointmentBookingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        public ActionResult Register() => View();

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "User registered successfully!";
                return RedirectToAction("Login");
            }
            return View(user);
        }
        public ActionResult Login() => View();
        [HttpPost]
        public ActionResult Login(User user)
        {
            var existingUser = _context.Users
                .FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);

            if (existingUser != null)
            {
                //Session["UserId"] = existingUser.Id;
                //Session["Username"] = existingUser.UserName;
                //return RedirectToAction("Book", "Appointment");
                HttpContext.Session.SetInt32("UserId", existingUser.Id);
                HttpContext.Session.SetString("Username", existingUser.UserName);
                HttpContext.Session.SetString("IsAdmin", existingUser.IsAdmin.ToString());//admin
                                                                                          //return RedirectToAction("Book", "Appointment");

                if (existingUser.IsAdmin)
                {
                    return RedirectToAction("ManageSlots", "Appointment");
                }
                else
                {
                    return RedirectToAction("Book", "Appointment");
                }

            }
           

            ViewBag.Message = "Invalid credentials";
            return View(user);

        }
        public ActionResult Logout()
        {
            //Session.Clear();
            //return RedirectToAction("Login");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
