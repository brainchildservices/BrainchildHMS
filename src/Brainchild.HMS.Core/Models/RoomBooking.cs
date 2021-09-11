using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class RoomBooking
{
    [Key]
    public int RoomBookingId { get; set; }

    public int BookingId { get; set; }
    public Booking  Booking { get; set;}
    
    public int RoomId { get; set; }
    public Room Room { get; set;}
        
}