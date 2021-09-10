using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Guest
{
    [Key]
    public int GuestId { get; set; }

    [Required]
    [Column(TypeName = "varchar(1000)")]
    public string Name { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string Address { get; set; }

    public string Email { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string PhoneNo { get; set; }
    
    
}