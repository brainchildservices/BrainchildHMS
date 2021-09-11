using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
    public DateTime CheckinDate { get; set; }

    [Required]
    public DateTime CheckoutDate { get; set; }
    
    [Column(TypeName = "int")]
    public BookingStatus Status { get; set; }

    public int IsCancelled { get; set; }

    public DateTime CancelleddDate { get; set; }


   public List<Billing> Billings { get; set; }
   public List<RoomBooking> RoomBookings { get; set; }
}