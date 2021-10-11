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
        double GetTotalCharges(int roomId);
        double GetTotalPayments(int roomId);
        void DoCheckOut(int roomId);
        void ChangeBookingStatus(int bookingId);
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
        public double GetTotalCharges(int roomId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select sum(ChargeAmount) as ChargeAmount from Charges where RoomId='" + roomId + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    return Convert.ToDouble(dr["ChargeAmount"]);
                }
            }

            return 0;            
        }
       public double GetTotalPayments(int roomId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select sum(Payments.PaymentAmount) as PaymentAmount from Payments inner join Billings on Payments.BillingId = Billings.BillingId inner join Rooms on Billings.RoomId = Rooms.RoomId where Rooms.RoomId = @roomId", con);
            cmd.Parameters.AddWithValue("@roomId", roomId);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    return Convert.ToDouble(dr["PaymentAmount"]);
                }
            }
            return 0;
        }
        public void DoCheckOut(int roomId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();          
            SqlCommand cmd = new SqlCommand("update Rooms set RoomStatus=0 where RoomId='" + roomId + "'", con);
            cmd.ExecuteNonQuery();           
        }
        public void ChangeBookingStatus(int bookingId)
        {
            int count = 0;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Bookings inner join RoomBookings on Bookings.BookingId = RoomBookings.BookingId inner join Rooms on RoomBookings.RoomId = Rooms.RoomId where Rooms.RoomStatus = 1 and Bookings.BookingId = @bookingId", con);
            cmd.Parameters.AddWithValue("@bookingID", bookingId);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    count++;
                }
            }
            con.Close();
            if (count <= 1)
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("update Bookings set Status=2 where BookingId='"+bookingId+"'", con);
                cmd1.ExecuteNonQuery();
                con.Close();

            }
        }
    }
}
