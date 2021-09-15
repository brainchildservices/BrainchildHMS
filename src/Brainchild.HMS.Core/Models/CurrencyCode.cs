using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brainchild.HMS.Core.Models
{
    public class CurrencyCode
    {
        [Key]
        public int CurrencyCodeId { get; set; }

        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string CurrencyCountry { get; set; }

        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string CurrencyCodes { get; set; }

        public int CurrencyNumber { get; set; } 

        [Column(TypeName = "varchar(1000)")]
        public string CurrencySymbol { get; set; }           

        public int HotelID { get; set; }
        public Hotel Hotel { get; set ;}    

        public List<Charge> Charges { get; set; }
           
    }
}