using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        // here we can "inject" our context service into the constructorcopy
        public HomeController(MyContext context)
        {
            _context = context;
        }
        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public User GetCurrentUser()
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (id == null)
            {
                return null;
            }
            return _context.Users.First(u => u.UserId == id);
        }
        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%Create User and Login User%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpPost("New/User")]
        public IActionResult NewUser(User newUser)
        {

            // %%%%%%%%%%%%%%Checks the email for duplicates
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                // %%%%%  error message%%%%%%%%%%%%%%%%%%%%%%%%%%
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Index");
                // You may consider returning to the View at this point
            }
            //%%%%%%%%%%%%%%%%%%%%Check For Validation%%%%%%%%%%%%%%%%%%%%%%%%%%
            if (ModelState.IsValid)
            {
                // %%%%%%%%%%%%%%%%%%%%%%%%%Hashes the password%%%%%%%%%%%%%%%%%%%%%%%%% 
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                //Save your user object to the database
                // %%%%%%%%%%%%% Adds the user to Database%%%%%%%%%%%%%%%%%
                _context.Add(newUser);

                _context.SaveChanges();
                // %%%%%%%%%%%%%%%Save user ID To Session%%%%%%%%%%%%%%%%%%%%%%%%%%%
                // %%%%%%%%%%%%%%%% declares a key is in green for the userid%%%%%%%
                HttpContext.Session.SetInt32("id", newUser.UserId);

                // %%%%%%%%%%%Redirect to a Dashboard%%%%%%%%%%%%%%%%%%%%%%%%
                return RedirectToAction("Dashboard");
            }
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%Validations must be Triggered will return View%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            return View("Index");
        }

        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%DashBoard%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpGet("Dashboard")]
        public ActionResult Dashboard()
        {
            // %%%%%%%%%Get the user info%%%%%%%%%%%%%%
            int? userid = HttpContext.Session.GetInt32("id");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userid);
            ViewBag.AllWeddings = _context
           .Weddings
           .Include(Wedding => Wedding.PostedBy)
           .Include(Wedding => Wedding.Guests)
           .OrderByDescending(wedding => wedding.Guests.Count);
            return View();
        }
        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Login %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpPost("Login")]
        public IActionResult Login(LoginUser userToLogin)
        {
            // %%%%%%%%% Look into DashBoard and make sure Email is in DB 
            var foundUser = _context.Users.FirstOrDefault(u => u.Email == userToLogin.LoginEmail);
            if (foundUser == null)
            {
                ModelState.AddModelError("LoginEmail", "Please check your email and password");
                return View("Index");
            }
            // %%%%%%%%%%%%%%% Makes sure Password match in database
            var hasher = new PasswordHasher<LoginUser>();

            var result = hasher.VerifyHashedPassword(userToLogin, foundUser.Password, userToLogin.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginEmail", "Please check your email and password");
                return View("Index");
            }
            // Set the key in session 
            HttpContext.Session.SetInt32("id", foundUser.UserId);
            return RedirectToAction("Dashboard");

        }
        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Log Out User %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Wedding Form page %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpGet("Wedding")]
        public IActionResult Wedding()
        {
            // %%%%%%%%%Get the user info%%%%%%%%%%%%%%
            int? userid = HttpContext.Session.GetInt32("id");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userid);

            return View();
        }

        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Create a Wedding %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpPost("Wedding/New")]
        public IActionResult NewWedding(Wedding NewWedding)
        {
            int? userid = HttpContext.Session.GetInt32("id");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userid);
            // %%%%%%% Checks the date is in Future%%%%%%%%
            if (NewWedding.Date <= DateTime.Now)
                ModelState.AddModelError("Date", "Please insure date is in future");
            if (ModelState.IsValid)
            {
                NewWedding.UserId = (int)HttpContext.Session.GetInt32("id");
                _context.Add(NewWedding);
                _context.SaveChanges();
                return Redirect($"/Wedding/{NewWedding.WeddingId}");
            }
            return View("Wedding");
        }

        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Display info for wedding %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpGet("Wedding/{WeddingId}")]

        public IActionResult WeddingInfo(int WeddingId)
        {
            int? userid = HttpContext.Session.GetInt32("id");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userid);
            // send id to ViewBag
            ViewBag.Wedding = _context.Weddings
            .Include(m => m.PostedBy)
            .Include(g => g.Guests)
            .ThenInclude(b => b.UserToWedding)
            .First(w => w.WeddingId == WeddingId);

            return View();
        }
        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Delete %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpPost("Wedding/{WeddingId}/Delete")]
        public IActionResult Delete(int WeddingId){
            var WeddingToDelete=_context
            .Weddings
            .First(w=> w.WeddingId == WeddingId);
            _context.Remove(WeddingToDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");

        }
        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%Delete Rsvp%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpPost("Guest/{WeddingId}/Guest/Delete")]
        public IActionResult DeleteRsvp(int WeddingId){
            // Get current user function
        var currentUser= GetCurrentUser();
        var RsvpDelete =_context
        .Guests
        .First(Rsvp => Rsvp.WeddingId == WeddingId && Rsvp.UserId == currentUser.UserId);
        _context.Remove(RsvpDelete);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
        }
        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%Add Guest to Rsvp%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [HttpPost("Guest/{WeddingId}/Guest")]
        public IActionResult AddGuest(int WeddingId)
        {
            var RsvpToAdd = new Guest{
                UserId=GetCurrentUser().UserId,
                WeddingId = WeddingId
            };
            _context.Add(RsvpToAdd);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
