﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Brainchild.HMS.Core.Models;

namespace Brainchild.HMS.Data.DTOs
{
    public class CancelBookingDTO
    {
        public string NoteDescription { get; set; }

    }
}
