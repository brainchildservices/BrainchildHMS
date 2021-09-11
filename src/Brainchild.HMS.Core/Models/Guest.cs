using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Guest
{
    [Key]
    public int GuestId { get; set; }

    [Required]
    [Column(TypeName = "varchar(1000)")]
    public string GuestName { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string GuestAddress { get; set; }

    public string GuestEmail { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string GuestPhoneNo { get; set; }
    
     public List<Booking> Bookings { get; set; }
    
}