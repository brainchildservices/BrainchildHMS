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
        guest.GuestName=this.GuestName;
        guest.GuestAddress=this.GuestAddress;
        guest.GuestEmail=this.GuestEmail;
        guest.GuestPhoneNo=this.GuestPhoneNo;
        guest.GuestCountry=this.GuestCountry;

        return guest;
    }
        public Guest Build(Guest guests)
        {
            Guest guest = new Guest();
            guest.GuestName = guests.GuestName;
            guest.GuestAddress = guests.GuestAddress;
            guest.GuestEmail = guests.GuestEmail;
            guest.GuestPhoneNo = guests.GuestPhoneNo;
            guest.GuestCountry = guests.GuestCountry;

            return guest;
        }

    }
}