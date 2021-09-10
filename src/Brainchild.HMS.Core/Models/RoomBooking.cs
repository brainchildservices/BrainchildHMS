using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class RoomBooking
{
    public int BookingDetailsId { get; set; }
    public int BookingId { get; set; }
    public int RoomId { get; set; }
        
}