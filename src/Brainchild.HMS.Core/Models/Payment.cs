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

        [ForeignKey("PaymentTypeId")]
        public PaymentType PaymentType { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public float PaymentAmount { get; set; }

        public float PaymentDescription { get; set; }

        [ForeignKey("BillingId")]
        public Billing Billing { get; set; }
    }
}