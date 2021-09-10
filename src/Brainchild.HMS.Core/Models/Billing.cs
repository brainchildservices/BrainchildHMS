
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Billing
{
    public int BillingId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public int BookingId { get; set; }

    public int PaymentId { get; set; }
    
}