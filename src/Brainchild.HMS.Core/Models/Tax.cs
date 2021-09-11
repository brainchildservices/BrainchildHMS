using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
public class Tax
{
    [Key]
    public int TaxId { get; set; }

    [Required]
    public int TaxPercentage { get; set; }

    [Column(TypeName = "varchar(1000)")]
    public string TaxDecription { get; set; }
    
    public int Status { get; set; }
    
}
}