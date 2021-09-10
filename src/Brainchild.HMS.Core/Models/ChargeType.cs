
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class ChargeType
{
    public int ChargeTypeId { get; set; }

    [Column(TypeName = "varchar(1000)")]
    public string ChargeTypeDescription { get; set; }       
    
}