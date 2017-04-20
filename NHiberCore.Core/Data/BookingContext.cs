using Microsoft.EntityFrameworkCore;
using NHiberCore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NHiberCore.Core.Data
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Room>().ToTable("Room");
        }
    }
}
