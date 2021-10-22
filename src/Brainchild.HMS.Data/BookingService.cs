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
    public interface IBookingService
    {
        int CreateGuest(GuestDTO guest);
        int CreateBooking(int guestId, BookingDTO booking);
        GuestDTO FindGuestByPhoneNumber(string phoneNo);
        List<Room> GetAvailableRooms(BookingDTO booking);
        void AddRoomBooking(int bookingId, int roomId);
        List<BookingDTO> SearchBooking(DateTime bookingDate, string guestPhoneNo, string guestName);

    }
    public class BookingService : IBookingService
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
            SqlCommand cmd = new SqlCommand("insert into Guests values(@GuestName,@GuestAddress,@GuestEmail,@GuestPhoneNo,@GuestCountry); SELECT SCOPE_IDENTITY()", con);
            cmd.Parameters.AddWithValue("@GuestName", guest.GuestName);
            cmd.Parameters.AddWithValue("@GuestAddress", guest.GuestAddress);
            cmd.Parameters.AddWithValue("@GuestEmail", guest.GuestEmail);
            cmd.Parameters.AddWithValue("@GuestPhoneNo", guest.GuestPhoneNo);
            cmd.Parameters.AddWithValue("@GuestCountry", guest.GuestCountry);            
            int guestId = Convert.ToInt32(cmd.ExecuteScalar());
            return guestId;

        }
        public int CreateBooking(int guestId, BookingDTO booking)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Bookings(GuestId,BookingDate,NoOfAdults,NoOfAChildren,CheckInDate,CheckOutDate,Status,HotelId) values(@gustid,@bookingDate,@NoOdAdult,@NoOfChildren,@checkin,@checkout,1,@hotelid);  SELECT SCOPE_IDENTITY()", con);
            cmd.Parameters.AddWithValue("@gustid", guestId);
            cmd.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@NoOdAdult", booking.NoOfAdults);
            cmd.Parameters.AddWithValue("@NoOfChildren", booking.NoOfAChildren);
            cmd.Parameters.AddWithValue("@checkin", booking.CheckInDate.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@checkout", booking.CheckOutDate.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@hotelid", booking.HotelId);            
            int bookingId = Convert.ToInt32(cmd.ExecuteScalar());
            return bookingId;
        }
        public GuestDTO FindGuestByPhoneNumber(string phoneNo)
        {
            GuestDTO gust = new GuestDTO();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Guests where GuestPhoneNo='" + phoneNo + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {

                while (dr.Read())
                {
                    gust.GuestId = Convert.ToInt32(dr["GuestId"]);
                    gust.GuestName = dr["GuestName"].ToString();
                    gust.GuestPhoneNo = dr["GuestPhoneNo"].ToString();
                    gust.GuestAddress = dr["GuestAddress"].ToString();
                    gust.GuestEmail = dr["GuestEmail"].ToString();
                    gust.GuestCountry = dr["GuestCountry"].ToString();
                }
            }

            return gust;
        }
        List<Room> availableRooms = new List<Room>();
        public List<Room> GetAvailableRooms(BookingDTO booking)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from (select RoomId, RoomNo from Rooms where HotelId='" + booking.HotelId + "') as T1 except select Rooms.RoomId, Rooms.RoomNo from Bookings  inner join RoomBookings on RoomBookings.bookingid = Bookings.BookingId  inner join Rooms on Rooms.RoomId = RoomBookings.RoomId where CheckInDate = '" + booking.CheckInDate.ToString("dd/MMMM/yyyy") + "' and CheckOutDate = '" + booking.CheckOutDate.ToString("dd/MMMM/yyyy") + "'and Bookings.HotelId = '" + booking.HotelId + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {                
                while (dr.Read())
                {
                    Room room = new Room();
                    room.RoomId = Convert.ToInt32(dr["RoomId"]);
                    room.RoomNo = dr["RoomNo"].ToString();
                    availableRooms.Add(room);
                }
            }

            return availableRooms;
        }

        public void AddRoomBooking(int bookingId, int roomId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into RoomBookings values('" + bookingId + "','" + roomId + "')", con);
            cmd.ExecuteNonQuery();
            con.Close();            
        }

        public List<BookingDTO> SearchBooking(DateTime bookingDate, string guestPhoneNo, string guestName)
        {
            List<BookingDTO> bookingList = new List<BookingDTO>();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT BookingId,CheckInDate,CheckOutDate,Guests.GuestId,GuestName FROM Bookings INNER JOIN Guests ON Bookings.GuestId = Guests.GuestId WHERE Bookings.BookingDate = '" + bookingDate+"' OR Guests.GuestPhoneNo = '"+guestPhoneNo+"' OR Guests.GuestName = '"+guestName+"'", sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    BookingDTO booking = new BookingDTO();
                    booking.BookingId = Convert.ToInt32(sqlDataReader["BookingId"]);
                    booking.CheckInDate = Convert.ToDateTime(sqlDataReader["CheckInDate"]);
                    booking.CheckOutDate = Convert.ToDateTime(sqlDataReader["CheckOutDate"]);                    
                    bookingList.Add(booking);
                }
            }
            
            return bookingList;
        }

    }
}
