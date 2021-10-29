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
        List<RoomPlanDTO> GetRoomPlan(DateTime fromDate, int hotelId);
    }
    public class HotelService : IHotelService
    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }
        List<RoomPlanDTO> roomPlanList = new List<RoomPlanDTO>();
        public List<RoomPlanDTO> GetRoomPlan(DateTime fromDate, int hotelId)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Establishing the connection
            sqlConnection.Open();            
            //Query for selecting the room details by date
            SqlCommand sqlCommand = new SqlCommand("SELECT Rooms.RoomNo, Rooms.RoomId, Bookings.BookingId, Guests.GuestName, RoomBookingId, RoomTypes.RoomTypeDesctiption as RoomType, CASE WHEN RoomBookingId IS NOT NULL AND @fromDate NOT between CheckInDate and CheckOutDate THEN 'VACANT' WHEN RoomBookingId IS NOT NULL AND @fromDate = CheckINDate  THEN 'CHEKCEDIN' WHEN RoomBookingId IS NOT NULL AND @fromDate = CheckOutDate  THEN 'CHEKCEDOUT' WHEN RoomBookingId IS NOT NULL AND @fromDate between CheckInDate and CheckOutDate THEN 'STAY OVER' END AS RoomStatus FROM Hotels INNER JOIN Bookings ON Bookings.HotelID = Hotels.HotelID LEFT OUTER JOIN RoomBookings  ON RoomBookings.BookingID = Bookings.bookingID INNER JOIN Rooms ON RoomBookings.RoomId = Rooms.RoomId INNER JOIN Guests ON Guests.GuestId = Bookings.GuestId INNER JOIN RoomTypes ON RoomTypes.RoomTypeId = Rooms.RoomTypeId WHERE Hotels.HotelID = @hotelId", sqlConnection);
            //Adding Parameters
            sqlCommand.Parameters.AddWithValue("@fromDate",fromDate);
            sqlCommand.Parameters.AddWithValue("@hotelId",hotelId);
            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    RoomPlanDTO room = new RoomPlanDTO();
                    room.RoomId = Convert.ToInt32(dr["RoomId"]);
                    room.RoomNo = dr["RoomNo"].ToString();
                    room.BookingId = Convert.ToInt32(dr["BookingId"]);
                    room.RoomType = dr["RoomType"].ToString();
                    room.RoomStatus = dr["RoomStatus"].ToString();
                    roomPlanList.Add(room);
                }
            }

            
            return roomPlanList;
        }
    }
}