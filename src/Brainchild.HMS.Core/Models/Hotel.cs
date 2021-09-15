using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Brainchild.HMS.Core.Models
{
public class Hotel
{
    [Key]
    public int HotelID { get; set; }

    public int TenantID { get; set; }

    [Required]
    [Column(TypeName = "varchar(1000)")]
    public string OwnerName { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string RegistrationNo { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string HotelEmail { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string HotelPhone { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string Place { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string GSTNo { get; set; }


}
}