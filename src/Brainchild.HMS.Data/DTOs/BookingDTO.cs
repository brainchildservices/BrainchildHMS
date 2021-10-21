using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Brainchild.HMS.Core.Models;
namespace Brainchild.HMS.Data.DTOs

{
    public class BookingDTO
    {

        public int BookingId { get; set; }

        public GuestDTO Guest { get; set; }

        public int GuestId { get; set; }

        public int NoOfAdults { get; set; }

        public int NoOfAChildren { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public int HotelId { get; set; }

        public int IsCancelled { get; set; }
        public int RoomId { get; set; }
        public RoomStatus RoomStatus { get; set; }

        public List<Room> Rooms { get; set; }

        public Booking Build(Guest guest, Guest gst)
        {
            Booking book = new Booking
            {
                Guest = guest == null ? gst : guest,
                NoOfAChildren = this.NoOfAChildren,
                NoOfAdults = this.NoOfAdults,
                CheckInDate = this.CheckInDate,
                CheckOutDate = this.CheckOutDate,
            };
            return book;
        }

    }
}