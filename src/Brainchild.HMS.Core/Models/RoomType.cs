using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
public class RoomType
{
    [Key]
    public int RoomTypeId { get; set; }

    [Required]
    [Column(TypeName = "varchar(1000)")]
    public string RoomTypeDesctiption { get; set; }

    public float RoomRate { get; set; }

    public List<Room> Rooms { get; set; }
   
    public int HotelID { get; set; }
    public Hotel Hotel { get; set ;}
}
}