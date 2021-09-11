using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
public class PaymentType
{
    [Key]
    public int PaymentTypeID { get; set; }

    [Required]
    [Column(TypeName = "varchar(1000)")]
    public int PaymentTypeDescription { get; set; }
}
 
    
}