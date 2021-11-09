using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
    public class RoomBooking
    {
        [Key]
        public int RoomBookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        public double RoomRate { get; set; }

    }
}