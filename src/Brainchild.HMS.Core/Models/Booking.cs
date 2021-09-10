using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Booking
{
    [Key]
    public int BookingId { get; set; }

    public int GuestId { get; set; }

    [Required]
    public DateTime BookingDate { get; set; }

    public int NoOfAdults { get; set; }

    public int NoOfAChildren { get; set; }

    [Required]
    public DateTime CheckinDate { get; set; }

    [Required]
    public DateTime CheckoutDate { get; set; }
    
    public int status { get; set; }

    public int IsCancelled { get; set; }

    public DateTime CancelDate { get; set; }
}