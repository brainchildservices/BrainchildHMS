using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainchild.HMS.Data.DTOs
{
    public class ChargeDTO
    {
        public int ChargeId { get; set; }
        public int ChargeTypeId { get; set; }
        public int CurrencyId { get; set; }
        public double ChargeAmount { get; set; }
        public int BookingId { get; set; }
        public int RoomId { get; set; }
    }
}
