
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class AdditionalCharge
{
    public int AdditionalCharge { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string AdditionalChargeType { get; set; }
    
    public float Tax { get; set; }
    
    
}