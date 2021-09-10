
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class PaymentType
{
    public int PaymentTypeID { get; set; }

    [Column(TypeName = "varchar(1000)")]
    public int PaymentType { get; set; }
       
    
}