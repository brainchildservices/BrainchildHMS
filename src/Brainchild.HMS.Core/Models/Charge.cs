
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Charge
{
    public int ChargeId { get; set; }

    public int ChargeTypeId { get; set; }    

    public float ChargeAmount { get; set; }   
    
}