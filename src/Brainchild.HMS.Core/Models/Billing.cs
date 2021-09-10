
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Billing
{
    public int BillingId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public int BookingId { get; set; }

    public int NoOfRooms { get; set; }

    public DateTime CheckinDate { get; set; }

    public DateTime CheckoutDate { get; set; }

    public float RoomCharges { get; set; }

    public float AdvancePayment { get; set; }

    public float PaidAmount { get; set; }
    
    public string BillStatus { get; set; }
}