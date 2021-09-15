using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
    [Index(nameof(RoomNo))]
public class Room
{
    [Key]
    public int RoomId { get; set; }

    public int RoomTypeID { get; set; }
    public RoomType RoomType { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string RoomNo { get; set; }

    [Column(TypeName = "int")]
    public RoomStatus RoomStatus { get; set; }
    
    public List<RoomBooking> RoomBookings { get; set; }
}
}