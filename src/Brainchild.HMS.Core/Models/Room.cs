using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Room
{
    public int RoomId { get; set; }
    public int RoomTypeID { get; set; }
    public string RoomNo { get; set; }
    public int RoomStatus { get; set; }
    
}