using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Brainchild.HMS.Core.Models;

namespace Brainchild.HMS.Data.DTOs
{
    public class BillingDTO
    {
        public int BookingId { get; set; }
        public DateTime BillingDate { get; set; }        
        public int HotelId { get; set; }
    }
}
