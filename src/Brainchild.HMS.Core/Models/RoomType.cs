using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class RoomType
{
    public int RoomTypeId { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string RoomType { get; set; }

    public float Rate { get; set; }
   
}