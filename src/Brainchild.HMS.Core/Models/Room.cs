using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Room
{
    public int RoomId { get; set; }

    public int RoomTypeID { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string RoomNo { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string RoomStatus { get; set; }
    
}