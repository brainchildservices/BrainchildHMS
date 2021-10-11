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
        double GetRoomRateByRoomId(int roomId, int hotelId);
      
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
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Bookings where BookingId='" + bookingId + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    booking.BookingId = Convert.ToInt32(dr["BookingId"]);
                    booking.GuestId = Convert.ToInt32(dr["GuestId"]);
                    booking.CheckInDate = Convert.ToDateTime(dr["CheckInDate"]);
                    booking.CheckOutDate = Convert.ToDateTime(dr["CheckOutDate"]);
                }
            }
            return booking;
        }
       
        public double GetRoomRateByRoomId(int roomId, int hotelId)
        {
            double roomRate = 0;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Rooms inner join RoomTypes on Rooms.RoomTypeId = RoomTypes.RoomTypeId where Rooms.RoomId = @roomId and RoomTypes.HotelId = @hotelId", con);
            cmd.Parameters.AddWithValue("@roomId",roomId);
            cmd.Parameters.AddWithValue("@hotelId", hotelId);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    roomRate = Convert.ToDouble(dr["RoomRate"]);
                }
            }

            return roomRate;
        }

    }
}
