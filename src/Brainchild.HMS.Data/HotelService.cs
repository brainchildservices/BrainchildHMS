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
        List<Charge> GetCharges(int bookingId, int hotelId);
    }
    public class HotelService : IHotelService
    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }
        List<Charge> charges = new List<Charge>();
        public List<Charge> GetCharges(int bookingId, int hotelId)
        {
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();
            //SQL query for fetching the total charges by roomId
            SqlCommand sqlCommand = new SqlCommand("", sqlConnection);
            //Executing the Query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object dr having values
            if (dr.HasRows)
            {
                //Reading the data
                while (dr.Read())
                {
                                       
                }
            }
            return charges;
        }
    }
}