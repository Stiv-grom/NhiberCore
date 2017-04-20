using NHiberCore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHiberCore.Core.Data
{
    public class DbInitializer
    {
        public static void Initialize(BookingContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Rooms.Any())
            {
                return;   // DB has been seeded
            }

            var rooms = new Room[]
            {
                new Room { Number = "803A", Floor = 8, HasPhone = true, HasTV = false },
                new Room { Number = "803B", Floor = 8, HasPhone = false, HasTV = true },
                new Room { Number = "403A", Floor = 4, HasPhone = false, HasTV = false },
                new Room { Number = "403E", Floor = 4, HasPhone = true, HasTV = true },
                new Room { Number = "503A", Floor = 5, HasPhone = true, HasTV = false },
                new Room { Number = "503B", Floor = 5, HasPhone = false, HasTV = true },
                new Room { Number = "703D", Floor = 7, HasPhone = false, HasTV = true },
                new Room { Number = "704", Floor = 7, HasPhone = true, HasTV = false },
                new Room { Number = "204C", Floor = 2, HasPhone = false, HasTV = true }
            };

            foreach (Room r in rooms)
            {
                context.Rooms.Add(r);
            }
            context.SaveChanges();

            var users = new User[]
            {
                new User { FirstName = "Hobo", LastName = "Bobo", Room = rooms[0] },
                new User { FirstName = "Bruce", LastName = "Wayne", Room = rooms[0] },
                new User { FirstName = "Lois", LastName = "Lane", Room = rooms[1] },
                new User { FirstName = "Rick", LastName = "Grimes", Room = rooms[2] },
                new User { FirstName = "Carl", LastName = "Grimes", Room = rooms[5] },
                new User { FirstName = "Ramona", LastName = "Flowers", Room = rooms[5] },
                new User { FirstName = "Dwight", LastName = "McCarthy", Room = rooms[4] },
                new User { FirstName = "Scott", LastName = "Pilgrim", Room = rooms[3] }
            };

            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
        }
    }
}
