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
    public interface IRoomService
    {
        List<Room> GetAvailableRooms(BookingDTO booking);
        void AddRoomBooking(int bookingId, int roomId);

    }
    public class RoomService : IRoomService
    {
        private readonly string connectionString;
        public RoomService(string connection)
        {
            connectionString = connection;
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

    }
}
