using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Brainchild.HMS.Data.DTOs;
using Brainchild.HMS.Core.Models;

namespace Brainchild.HMS.Data
{
    public interface IHotelService
    {
        
    }
    public class HotelService : IHotelService
    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }
    }
}