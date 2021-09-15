using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int PaymentTypeID { get; set; }
    public PaymentType PaymentType { get; set; }

    public float PaymentAmount { get; set; }

    public float PaymentAdvance { get; set; }    


    public int? BillingId { get; set; }
    public Billing Billing { get; set; }
}
}