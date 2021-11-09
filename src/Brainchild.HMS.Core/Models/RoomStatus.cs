using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Brainchild.HMS.Core.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoomStatus
    {
        Vacant,
        Occupied,
        OutOfOrder,
        Dirty
    }
}