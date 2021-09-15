using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
public class Charge
{
    [Key]
    public int ChargeId { get; set; }

    [ForeignKey("ChargeTypeId")]   
    public ChargeType ChargeType { get; set; }    

    [ForeignKey("CurrencyCodeId")]
    public Currency Currency { get; set;}

    [Required]
    public float ChargeAmount { get; set; }   

   
    
}
}