using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brainchild.HMS.Core.Models
{
    public enum RoomStatus
{
    Occupied, 
    Vacant, 
    OutOfOrder     
}
}