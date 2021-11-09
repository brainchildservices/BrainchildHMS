using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainchild.HMS.Data.DTOs
{
    public class AvailableRoomDTO
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string RoomType { get; set; }
        public string RoomStatus { get; set; }
        
    }
}
