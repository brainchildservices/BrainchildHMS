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
    public interface IChargeService
    {
        void AddCharges(ChargeDTO charge);
    }
    public class ChargeService : IChargeService
    {
        private readonly string connectionString;
        public ChargeService(string connection)
        {
            connectionString = connection;
        }
        public void AddCharges(ChargeDTO charge)
        {
            //creating an sqlconnection object.
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //opening the connection.
            sqlConnection.Open();
            //SQL query for inserting the charges
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Charges VALUES(@chargeTypeId,@currencyId,@chargeAmount,@bookingId,@roomId);SELECT SCOPE_IDENTITY()", sqlConnection);
            //Adding parameters
            sqlCommand.Parameters.AddWithValue("@chargeTypeId", charge.ChargeTypeId);
            sqlCommand.Parameters.AddWithValue("@currencyId", charge.CurrencyId);
            sqlCommand.Parameters.AddWithValue("@chargeAmount", charge.ChargeAmount);
            sqlCommand.Parameters.AddWithValue("@bookingId", charge.BookingId);
            sqlCommand.Parameters.AddWithValue("@roomId", charge.RoomId);
            //Executing the query and storing the paymentId 
            charge.ChargeId = Convert.ToInt32(sqlCommand.ExecuteScalar());
            //Closing the connection
            sqlConnection.Close();
        }
    }
}
