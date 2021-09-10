using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class RoomType
{
    public int RoomTypeId { get; set; }

    [Column(TypeName = "varchar(1000)")]
    public string RoomTypeDesctiption { get; set; }

    public float RoomRate { get; set; }
   
}