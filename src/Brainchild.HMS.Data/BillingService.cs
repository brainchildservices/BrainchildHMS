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
    public interface IBillingService
    {
        BookingDTO GetBookingDetails(int bookingId);
        double GetRoomRateByRoomId(int roomId, int bookingId);
        double GetTotalCharges(int roomId);
        double GetTotalPayments(int roomId);
        void DoCheckOut(int roomId);
       
    }
    public class BillingService:IBillingService
    {
        private readonly string connectionString;
        public BillingService(string connection)
        {
            connectionString = connection;
        }
        public BookingDTO GetBookingDetails(int bookingId)
        {
            BookingDTO booking = new BookingDTO();
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();
            //SQL query for fetching the booking details
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Bookings WHERE BookingId='" + bookingId + "'", sqlConnection);
            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    //Storing the data to booking object
                    booking.BookingId = Convert.ToInt32(dr["BookingId"]);
                    booking.GuestId = Convert.ToInt32(dr["GuestId"]);
                    booking.CheckInDate = Convert.ToDateTime(dr["CheckInDate"]);
                    booking.CheckOutDate = Convert.ToDateTime(dr["CheckOutDate"]);
                }
            }
            //Returning the booking object
            return booking;
        }
       
        public double GetRoomRateByRoomId(int roomId, int bookingId)
        {
            double roomRate = 0;
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();
            //SQL query for selecting the room rate by roomId
            SqlCommand sqlCommand = new SqlCommand("SELECT RoomRate from RoomBookings where RoomId=@roomId AND BookingId=@bookingId", sqlConnection);
            //Adding parameters
            sqlCommand.Parameters.AddWithValue("@roomId",roomId);
            sqlCommand.Parameters.AddWithValue("@bookingId", bookingId);
            //Executing the query
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object dr having data
            if (dr.HasRows)
            {
                //Reading the data riw by row
                while (dr.Read())
                {
                    //Storing the roomRate
                    roomRate = Convert.ToDouble(dr["RoomRate"]);
                }
            }
            //Returning the roomRate
            return roomRate;
        }
        public double GetTotalCharges(int roomId)
        {
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();
            //SQL query for fetching the total charges by roomId
            SqlCommand sqlCommand = new SqlCommand("SELECT SUM(ChargeAmount) AS ChargeAmount FROM Charges WHERE RoomId='" + roomId + "'", sqlConnection);
            //Executing the Query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object dr having values
            if (dr.HasRows)
            {
                //Reading the data
                while (dr.Read())
                {
                    //Return the totalCharge
                    return Convert.ToDouble(dr["ChargeAmount"]);
                }
            }

            return 0;            
        }
       public double GetTotalPayments(int roomId)
        {
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();
            //SQL query to calculate the total payments done by roomId
            SqlCommand sqlCommand = new SqlCommand("SELECT SUM(Payments.PaymentAmount) AS PaymentAmount FROM Payments INNER JOIN Billings ON Payments.BillingId = Billings.BillingId INNER JOIN Rooms ON Billings.RoomId = Rooms.RoomId WHERE Rooms.RoomId = @roomId", sqlConnection);
            //Adding parameters
            sqlCommand.Parameters.AddWithValue("@roomId", roomId);
            //Executing the query
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //checking the object dr having data
            if (dr.HasRows)
            {
                //reading the data
                while (dr.Read())
                {
                    //Returning the totalPayements
                    return Convert.ToDouble(dr["PaymentAmount"]);
                }
            }
            return 0;
        }
        public void DoCheckOut(int roomId)
        {
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();       
            //SQL Query for update the room status
            SqlCommand sqlCommand = new SqlCommand("UPDATE Rooms SET RoomStatus=3 WHERE RoomId='" + roomId + "'", sqlConnection);
            //Executing the query
            sqlCommand.ExecuteNonQuery();
            //Closing the connection
            sqlConnection.Close();
        }
        
    }
}
