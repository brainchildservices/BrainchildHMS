using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Brainchild.HMS.Core.Models;
namespace Brainchild.HMS.Data.Context
{
    public class BrainchildHMSDbContext : DbContext
    {

         public BrainchildHMSDbContext(DbContextOptions<BrainchildHMSDbContext> options)
        : base(options)
    {
    }

        public DbSet<Billing> Billings { get; set; } 
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<ChargeType> ChargeTypes { get; set; }    
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomBooking> RoomBookings { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Tax> Taxes { get; set; }

    }   
    }