using Brainchild.HMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainchild.HMS.Data.DTOs
{
    public class CheckoutDetailsDTO
    {
        public int BookingId { get; set; }
        public string GuestName { get; set; }
        public string RoomNo { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double RoomRate { get; set; }       
        public List<Charge> charges { get; set; }
    }
}
