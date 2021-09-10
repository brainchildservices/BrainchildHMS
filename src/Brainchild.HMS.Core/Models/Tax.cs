using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Tax
{
    public int TaxId { get; set; }
    public int TaxPercentage { get; set; }
    [Column(TypeName = "varchar(100)")]
    public string TaxDecription { get; set; }
    public int Status { get; set; }
    
}