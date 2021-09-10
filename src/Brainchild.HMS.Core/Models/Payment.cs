
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Payment
{
    public int PaymentId { get; set; }

    public int PaymentTypeID { get; set; }

    public float RoomCharges { get; set; }

    public float AdvancePayment { get; set; }    
    
}