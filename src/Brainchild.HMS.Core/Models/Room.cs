using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Brainchild.HMS.Core.Models
{
    [Index(nameof(RoomNo))]
public class Room
{
    [Key]
    public int RoomId { get; set; }

   [ForeignKey("RoomTypeId")]
    public RoomType RoomType { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string RoomNo { get; set; }

    [Column(TypeName = "int")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomStatus RoomStatus { get; set; }
    
    [ForeignKey("HotelId")]   
    public Hotel Hotel { get; set ;}
}
}