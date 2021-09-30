using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Brainchild.HMS.API.DTOs;
using Brainchild.HMS.Core.Models;

namespace Brainchild.HMS.Data
{
    public interface IBookingService
    {
        int CreateGuest(GuestDTO guest);
        int CreateBooking(int guestId, BookingDTO booking);
        int FindGuestByPhoneNumber(string phoneNo);
        List<Room> CheckRoomAvailability(BookingDTO booking);
        int GetId(string id,string tableName);
        void AddRoomBooking(int bookingId, int roomId);

    }
    public class BookingService:IBookingService
    {       
        private readonly string connectionString;
        public BookingService(string connection)
        {
            connectionString = connection;
        }
        public int CreateGuest(GuestDTO guest)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Guests values(@GuestName,@GuestAddress,@GuestEmail,@GuestPhoneNo,@GuestCountry)", con);
            cmd.Parameters.AddWithValue("@GuestName", guest.GuestName);
            cmd.Parameters.AddWithValue("@GuestAddress", guest.GuestAddress);
            cmd.Parameters.AddWithValue("@GuestEmail", guest.GuestEmail);
            cmd.Parameters.AddWithValue("@GuestPhoneNo", guest.GuestPhoneNo);
            cmd.Parameters.AddWithValue("@GuestCountry", guest.GuestCountry);
            int inserted=cmd.ExecuteNonQuery();
            if (inserted > 0)
            {
                return GetId("GuestId", "Guests");
            }
            return 0;
            
        }
        public int CreateBooking(int guestId, BookingDTO booking)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Bookings(GuestId,BookingDate,NoOfAdults,NoOfAChildren,CheckInDate,CheckOutDate,Status,HotelId) values(@gustid,@bookingDate,@NoOdAdult,@NoOfChildren,@checkin,@checkout,1,@hotelid)", con);
            cmd.Parameters.AddWithValue("@gustid", guestId);
            cmd.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@NoOdAdult", booking.NoOfAdults);
            cmd.Parameters.AddWithValue("@NoOfChildren", booking.NoOfAChildren);
            cmd.Parameters.AddWithValue("@checkin", booking.CheckInDate.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@checkout", booking.CheckOutDate.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@hotelid", booking.HotelId);
            int inserted = cmd.ExecuteNonQuery();
            if (inserted > 0)
            {
                return GetId("BookingId", "Bookings");
            }
            return 0;
        }
        public int FindGuestByPhoneNumber(string phoneNo)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Guests where GuestPhoneNo='" + phoneNo + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["GuestId"]);
                    return id;
                }
            }

            return 0;
        }
        List<Room> availableRooms = new List<Room>();
        public List<Room> CheckRoomAvailability(BookingDTO booking)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from (select RoomId, RoomNo from Rooms where HotelId='" + booking.HotelId + "') as T1 except select Rooms.RoomId, Rooms.RoomNo from Bookings  inner join RoomBookings on RoomBookings.bookingid = Bookings.BookingId  inner join Rooms on Rooms.RoomId = RoomBookings.RoomId where CheckInDate = '" + booking.CheckInDate.ToString("dd/MMMM/yyyy") + "' and CheckOutDate = '" + booking.CheckOutDate.ToString("dd/MMMM/yyyy") + "'and Bookings.HotelId = '" + booking.HotelId + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                Room room = new Room();               
                while (dr.Read())
                {  
                    room.RoomId =Convert.ToInt32(dr["RoomId"]);
                    room.RoomNo = dr["RoomNo"].ToString();
                    availableRooms.Add(room);
                }
            }

            return availableRooms;
        }
        public int GetId(string id,string tableName)
        {
            string query="select max("+id+") from "+tableName+"";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {                     
                    return Convert.ToInt32(dr[0]);
                }
            }

            return 0;
        }
        public void AddRoomBooking(int bookingId, int roomId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into RoomBookings values('" + bookingId + "','" + roomId + "')", con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
