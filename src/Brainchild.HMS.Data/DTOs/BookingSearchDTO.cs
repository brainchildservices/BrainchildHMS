using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainchild.HMS.Data.DTOs
{
    public class BookingSearchDTO
    {
        public DateTime BookingDate { get; set; }
        public string GuestPhoneNo { get; set; }
        public string GuestName { get; set; }
    }
}
