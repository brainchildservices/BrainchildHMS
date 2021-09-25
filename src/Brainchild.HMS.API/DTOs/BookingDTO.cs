using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Brainchild.HMS.Core.Models;
namespace Brainchild.HMS.API.DTOs

{
public class BookingDTO
{
    
    public int BookingId { get; set; }
   
    public Guest Guest { get; set ;}

    public int NoOfAdults { get; set; }

    public int NoOfAChildren { get; set; }
 
    public DateTime CheckInDate { get; set; }
 
    public DateTime CheckOutDate { get; set; }
  
    public int HotelId { get; set ;}

    public int[] RoomId { get; set;  }

    

}
}