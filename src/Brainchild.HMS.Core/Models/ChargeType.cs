using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
public class ChargeType
{
    [Key]
    public int ChargeTypeId { get; set; }

    [Required]
    [Column(TypeName = "varchar(1000)")]
    public string ChargeTypeDescription { get; set; }       
    
}  
}