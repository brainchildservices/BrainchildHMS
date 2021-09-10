using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Tax
{
    public int TaxId { get; set; }

    [Required]
    public int TaxPercentage { get; set; }

    [Column(TypeName = "varchar(1000)")]
    public string TaxDecription { get; set; }
    
    public int Status { get; set; }
    
}