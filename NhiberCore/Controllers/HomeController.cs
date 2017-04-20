using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHiberCore.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace NhiberCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookingContext _context;

        public HomeController(BookingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.OrderBy(x => x.Number).ToListAsync());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [Route("rooms")]
        public async Task<JsonResult> RoomDetails(int id)
        {
            //if (String.IsNullOrEmpty(roomNumber))
            //{
            //    return NotFound();
            //}

            var user = await _context.Users
                .Include("Room")
                .Where(x => x.Room.Id == id).Select(x => new { x.ID, x.FirstName, x.LastName, x.Room.Number }).ToListAsync();
            return Json(user);
        }

        public async Task<JsonResult> ChangeName(string name, int pk, string value)
        {
            var userToUpdate = await _context.Users.SingleOrDefaultAsync(m => m.ID == pk);
            if (name.Contains("userFirstName"))
            {
                userToUpdate.FirstName = value;
            }
            else
            {
                userToUpdate.LastName = value;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }

            return Json("error");
        }

        public async Task<JsonResult> DeleteUser(int id)
        {
            id = id * 100;
            var user = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Json("OK");
            }

            return Json("User not found");
        }
    }
}
