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
        List<RoomDTO> GetRoomDetails(int bookingId, int hotelId);

    }
    public class BillingService : IBillingService
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
        List<RoomDTO> roomDetails = new List<RoomDTO>();
        public List<RoomDTO> GetRoomDetails(int bookingId, int hotelId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from RoomBookings inner join Rooms on RoomBookings.RoomId = Rooms.RoomId inner join RoomTypes on Rooms.RoomTypeId = RoomTypes.RoomTypeId where RoomBookings.BookingId = @bookingId and RoomTypes.HotelId= @hotelId", con);
            cmd.Parameters.AddWithValue("@bookingId", bookingId);
            cmd.Parameters.AddWithValue("@hotelId", hotelId);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    RoomDTO room = new RoomDTO();
                    room.RoomId = Convert.ToInt32(dr["RoomId"]);
                    room.RoomNo = dr["RoomNo"].ToString();
                    room.RoomRate = Convert.ToDouble(dr["RoomRate"]);
                    roomDetails.Add(room);
                }
            }

            return roomDetails;
        }

    }
}
