using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public enum BookingStatus
{
    Booked,
    StayOver,
    Checkedout,
    Cancelled        
}