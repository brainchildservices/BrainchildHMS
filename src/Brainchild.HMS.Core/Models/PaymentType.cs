
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class PaymentType
{
    [Key]
    public int PaymentTypeID { get; set; }

    [Required]
    [Column(TypeName = "varchar(1000)")]
    public int PaymentTypeDescription { get; set; }

    public List<Payment> Payments { get; set; } 
    
}