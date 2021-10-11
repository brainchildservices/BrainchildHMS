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

        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        [Required]
        public float ChargeAmount { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

    }
}