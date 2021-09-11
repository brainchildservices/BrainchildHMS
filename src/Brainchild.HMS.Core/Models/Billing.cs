using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
public class Billing
{
    [Key]
    public int BillingId { get; set; }

    [Required]
    public DateTime BillingDate { get; set; }
    
    public int BookingId { get; set; }
    public Booking Booking { get; set ;}

       
    public List<Payment> Payments { get; set; }
}
}