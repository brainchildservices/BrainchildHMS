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

        List<ChargeDTO> GetCharges(int bookingId, int roomId);
        CheckoutDetailsDTO GetCheckoutDetails(int bookingId, int roomId, int hotelId);

    }
    public class HotelService : IHotelService
    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }

        List<ChargeDTO> charges = new List<ChargeDTO>();
        public List<ChargeDTO> GetCharges(int bookingId, int roomId)
        {
            CheckoutDetailsDTO checkoutDetails = new CheckoutDetailsDTO();
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();
            //SQL query for fetching the charges
            SqlCommand sqlCommand = new SqlCommand("SELECT ChargeId,ChargeTypeDescription,ChargeAmount FROM Charges INNER JOIN ChargeTypes ON ChargeTypes.ChargeTypeId = Charges.ChargeTypeId WHERE BookingId='"+bookingId+"' AND RoomId='"+roomId+"'", sqlConnection);
            //Executing the Query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object dr having values
            if (dr.HasRows)
            {
                //Reading the data
                while (dr.Read())
                {
                    ChargeDTO charge = new ChargeDTO();
                    charge.ChargeId = Convert.ToInt32(dr["ChargeId"]);
                    charge.ChargeDescription = dr["ChargeTypeDescription"].ToString();
                    charge.ChargeAmount = float.Parse(dr["ChargeAmount"].ToString());                    
                    charges.Add(charge);                   

                }
            }
            return charges;
        }
        public CheckoutDetailsDTO GetCheckoutDetails(int bookingId, int roomId, int hotelId)
        {
            CheckoutDetailsDTO checkoutDetails = new CheckoutDetailsDTO();
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();
            //SQL query for fetching the checout details
            SqlCommand sqlCommand = new SqlCommand("SELECT Bookings.BookingId, Guests.GuestName, Bookings.CheckInDate, Bookings.CheckOutDate, RoomBookings.RoomRate, Rooms.RoomNo FROM Bookings INNER JOIN RoomBookings ON Bookings.BookingId = RoomBookings.BookingId INNER JOIN Rooms ON Rooms.RoomId = RoomBookings.RoomId INNER JOIN Guests ON Guests.GuestId = Bookings.GuestId WHERE Bookings.BookingId = '"+bookingId+"' AND Bookings.HotelId = '"+hotelId+"' AND Rooms.RoomId = '"+roomId+"'", sqlConnection);
            //Executing the Query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object dr having values
            if (dr.HasRows)
            {
                //Reading the data
                while (dr.Read())
                {
                    checkoutDetails.BookingId = Convert.ToInt32(dr["BookingId"]);
                    checkoutDetails.GuestName = dr["GuestName"].ToString();
                    checkoutDetails.CheckInDate = Convert.ToDateTime(dr["CheckInDate"]);
                    checkoutDetails.CheckOutDate = Convert.ToDateTime(dr["CheckOutDate"]);
                    checkoutDetails.RoomNo = dr["RoomNo"].ToString();
                    checkoutDetails.RoomRate = Convert.ToDouble(dr["RoomRate"]);                             
                }
            }
            return checkoutDetails;

        }
    }
}