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

    public int ChargeTypeId { get; set; }
    public ChargeType ChargeType { get; set; }    

    public int? CurrencyCodeId { get; set; }
    public CurrencyCode CurrencyCode { get; set;}

    [Required]
    public float ChargeAmount { get; set; }   

   
    
}
}