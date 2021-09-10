using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Booking
{
    public int BookingId { get; set; }

    public int GuestId { get; set; }

    public DateTime BookingDate { get; set; }

    public int NoOfAdults { get; set; }

    public int NoOfAChildren { get; set; }

    public DateTime CheckinDate { get; set; }

    public DateTime CheckoutDate { get; set; }
    
    public int status { get; set; }
}