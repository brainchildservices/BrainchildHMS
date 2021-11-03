using Brainchild.HMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainchild.HMS.Data.DTOs
{
    class CheckoutDetailsDTO
    {
        public int BookingId { get; set; }
        public int GuestName { get; set; }
        public string RoomNo { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double RoomRate { get; set; }
        List<Charge> Charges { get; set; }
    }
}
