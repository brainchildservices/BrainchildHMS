using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class RoomType
{
    [Key]
    public int RoomTypeId { get; set; }

    [Required]
    [Column(TypeName = "varchar(1000)")]
    public string RoomTypeDesctiption { get; set; }

    public float RoomRate { get; set; }

    public List<Room> Rooms { get; set; }
   
}