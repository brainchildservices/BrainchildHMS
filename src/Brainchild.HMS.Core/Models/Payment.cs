
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Payment
{
    public int PaymentId { get; set; }

    public int PaymentTypeID { get; set; }

    public float PaymentAmount { get; set; }

    public float PaymentAdvance { get; set; }    
    
}