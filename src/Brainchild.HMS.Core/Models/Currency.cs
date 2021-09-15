using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brainchild.HMS.Core.Models
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }

        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string CurrencyCountry { get; set; }

        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string CurrencyCode { get; set; }

        public int CurrencyNumber { get; set; } 

        [Column(TypeName = "varchar(1000)")]
        public string CurrencySymbol { get; set; }           

       
           
    }
}