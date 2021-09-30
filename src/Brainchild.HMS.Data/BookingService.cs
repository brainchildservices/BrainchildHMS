using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Brainchild.HMS.API.DTOs;

namespace Brainchild.HMS.Data
{
    public class BookingService
    {
        
        private readonly string cs;
        public BookingService(string connection)
        {
             cs = connection;
        }
        public void insert_guest(GuestDTO guest)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Guests values(@GuestName,@GuestAddress,@GuestEmail,@GuestPhoneNo,@GuestCountry)", con);
            cmd.Parameters.AddWithValue("@GuestName", guest.GuestName);
            cmd.Parameters.AddWithValue("@GuestAddress", guest.GuestAddress);
            cmd.Parameters.AddWithValue("@GuestEmail", guest.GuestEmail);
            cmd.Parameters.AddWithValue("@GuestPhoneNo", guest.GuestPhoneNo);
            cmd.Parameters.AddWithValue("@GuestCountry", guest.GuestCountry);
            cmd.ExecuteNonQuery();
        }
         public void newbooking(int guestid, BookingDTO booking)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Bookings(GuestId,BookingDate,NoOfAdults,NoOfAChildren,CheckInDate,CheckOutDate,Status,HotelId) values(@gustid,@bookingDate,@NoOdAdult,@NoOfChildren,@checkin,@checkout,1,@hotelid)", con);
            cmd.Parameters.AddWithValue("@gustid", guestid);
            cmd.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@NoOdAdult", booking.NoOfAdults);
            cmd.Parameters.AddWithValue("@NoOfChildren", booking.NoOfAChildren);
            cmd.Parameters.AddWithValue("@checkin", booking.CheckInDate.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@checkout", booking.CheckOutDate.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@hotelid", booking.HotelId);
            cmd.ExecuteNonQuery();

        }
        public int chechGuest(string phone)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Guests where GuestPhoneNo='" + phone + "'", con);
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
       
        public int checkRoom(BookingDTO booking)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from (select RoomId, RoomNo from Rooms where HotelId='"+booking.HotelId+"') as T1 except select Rooms.RoomId, Rooms.RoomNo from Bookings  inner join RoomBookings on RoomBookings.bookingid = Bookings.BookingId  inner join Rooms on Rooms.RoomId = RoomBookings.RoomId where CheckInDate = '" + booking.CheckInDate.ToString("dd/MMMM/yyyy")+"' and CheckOutDate = '"+booking.CheckOutDate.ToString("dd/MMMM/yyyy")+"'and Bookings.HotelId = '"+booking.HotelId+"'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["RoomId"]);
                    return id;
                }
            }

            return 0;
        }
        public int get_bookingId()
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select max(BookingId) from Bookings", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr[0]);
                    return id;
                }
            }

            return 0;
        }
        public void insert_RoomBooking(int bookingid,int roomid)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into RoomBookings values('"+bookingid+"','"+roomid+"')", con);            
            cmd.ExecuteNonQuery();
        }
    }
}
