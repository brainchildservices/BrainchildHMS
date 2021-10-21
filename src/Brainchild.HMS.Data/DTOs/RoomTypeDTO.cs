using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Data.DTOs
{
public class RoomTypeDTO
{
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
      public int RoomTypeId { get; set; }
    

}
}