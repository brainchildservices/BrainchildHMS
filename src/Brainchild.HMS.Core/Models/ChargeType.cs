
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class ChargeType
{
    [Key]
    public int ChargeTypeId { get; set; }

    [Required]
    [Column(TypeName = "varchar(1000)")]
    public string ChargeTypeDescription { get; set; }       
    
}