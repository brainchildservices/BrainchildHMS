using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brainchild.HMS.Core.Models
{
public class Booking
{
    [Key]
    public int BookingId { get; set; }

    public int GuestId { get; set; }
    public Guest Guest { get; set ;}

    [Required]
    public DateTime BookingDate { get; set; }

    public int NoOfAdults { get; set; }

    public int NoOfAChildren { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }
    
    [Column(TypeName = "int")]
    public BookingStatus Status { get; set; }

    public int IsCancelled { get; set; }

    public DateTime CancelleddDate { get; set; }

    public int? HotelID { get; set; }
    public Hotel Hotel { get; set ;}

   public List<Billing> Bills { get; set; }
   public List<RoomBooking> RoomBookings { get; set; }
}
}