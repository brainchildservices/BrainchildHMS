using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Brainchild.HMS.Core.Models;
namespace Brainchild.HMS.API.DTOs
{
public class GuestDTO
{
   
    public int GuestId { get; set; }
   
    public string GuestName { get; set; }
    
    public string GuestAddress { get; set; }

    public string GuestEmail { get; set; }

    public string GuestPhoneNo { get; set; }

    public string GuestCountry { get; set; }
    
    public Guest Build()
    {
        Guest guest=new Guest();
        guest.GuestName=this.Guest.GuestName;
        guest.GuestAddress=this.Guest.GuestAddress;
        guest.GuestEmail=this.Guest.GuestEmail;
        guest.GuestPhoneNo=this.Guest.GuestPhoneNo;
        guest.GuestCountry=this.Guest.GuestCountry;

        return guest;
    }
    
}
}