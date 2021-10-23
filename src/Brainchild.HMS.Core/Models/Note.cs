using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        public string NoteDescription { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
    }
}
