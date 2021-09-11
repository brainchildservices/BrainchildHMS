
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int PaymentTypeID { get; set; }
    public PaymentType PaymentType { get; set; }

    public float PaymentAmount { get; set; }

    public float PaymentAdvance { get; set; }    


    public List<Billing> Billings { get; set; }
}