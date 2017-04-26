using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHiberCore.Core.Data;
using Microsoft.EntityFrameworkCore;
using NHiberCore.Core.Models;

namespace NhiberCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookingContext _context;
        private List<Room> allRooms;

        public HomeController(BookingContext context)
        {
            _context = context;
            allRooms = _context.Rooms.OrderBy(x => x.Number).ToList();
        }

        public async Task<IActionResult> Index()
        {

            ViewData["Users"] = await _context.Users.Include("Room").Select(x => new UserGUI(x.ID, x.FirstName, x.LastName, x.Room.Number)).ToListAsync();

            ViewData["Rooms"] = allRooms;
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

        //[Route("rooms")]
        public async Task<IActionResult> RoomDetails(int id)
        {
            //var user = await _context.Users
            //    .Include("Room")
            //    .Where(x => x.Room.Id == id).Select(x => new { x.ID, x.FirstName, x.LastName, x.Room.Number }).ToListAsync();

            //return Json(user);

            var users = await _context.Users
                .Include("Room")
                .Where(x => x.Room.Id == id).Select(x => new UserGUI (x.ID, x.FirstName, x.LastName, x.Room.Number)).ToListAsync();

            ViewData["Users"] = users;
            ViewData["Rooms"] = allRooms;
            return View("Index");
            //return PartialView("UsersPartial", users);
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
            //id = id * 100;
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

        public async Task<JsonResult> AddUser(string firstName, string lastName, string roomNumber)
        {

            var user = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.FirstName == firstName && m.LastName == lastName && m.Room.Number == roomNumber);

            if (user == null)
            {
                var userRoom = await _context.Rooms.AsNoTracking().SingleOrDefaultAsync(m => m.Number == roomNumber);

                if (userRoom != null)
                {
                    var newUser = new User { FirstName = firstName, LastName = lastName, Room = userRoom };

                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    return Json(new UserGUI(0, newUser.FirstName, newUser.LastName, newUser.Room.Number));
                }
                else
                {
                    return Json("Room is not found");
                }
            }

            return Json("User duplicate");
        }
        public async Task<JsonResult> Users()
        {

            var users = await _context.Users
                .Include("Room")
                .Select(x => new UserGUI(x.ID, x.FirstName, x.LastName, x.Room.Number)).ToListAsync();

            return Json(users);
        }

        public async Task<IActionResult> RoomUsers(int id)
        {
            var users = await _context.Users
                .Include("Room")
                .Where(x => x.Room.Id == id).Select(x => new UserGUI(x.ID, x.FirstName, x.LastName, x.Room.Number)).ToListAsync();

            return Json(users);
        }

        public async Task<IActionResult> RoomDetailsPage(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);

            return View("RoomDetails", room);
        }

        public async Task<IActionResult> SaveRoomChanges(int roomId, string roomNumber, string description)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);

            if (room != null)
            {
                room.Number = roomNumber;
                room.Description = description;
                await _context.SaveChangesAsync();
                return Json("ok");
            }

            return Json("Room wasn't found");
        }
    }
}
