using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Guest
{
    public int GuestId { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string Name { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string Address { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string Email { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string PhoneNo { get; set; }
    
    
}