using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainchild.HMS.Data.DTOs
{
    public class RoomPlanDTO
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int RoomId { get; set; }
        public string RoomNo { get; set; }
        public int BookingId { get; set; }
        public string RoomType { get; set; }
        public string GuestName { get; set; }
        public string RoomStatus { get; set; }
    }
}
