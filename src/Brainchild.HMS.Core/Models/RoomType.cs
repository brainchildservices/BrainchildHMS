using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class RoomType
{
    public int RoomTypeId { get; set; }
    public string RoomType { get; set; }
    public float Rate { get; set; }
    public int Status { get; set; }    
}