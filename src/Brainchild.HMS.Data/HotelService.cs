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
        List<RoomPlanDTO> GetRoomPlan(DateTime fromDate,DateTime toDate, int hotelId);
    }
    public class HotelService : IHotelService
    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }
        List<RoomPlanDTO> roomPlanList = new List<RoomPlanDTO>();
        public List<RoomPlanDTO> GetRoomPlan(DateTime fromDate, DateTime toDate, int hotelId)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Establishing the connection
            sqlConnection.Open();
            //creating temptable
            SqlCommand command = new SqlCommand("CREATE TABLE #TempTable(HotelId int, SearchDate date)", sqlConnection);
            command.ExecuteNonQuery();
            //storing the fromdate to date variable
            DateTime date = fromDate;
            //inserting the dates to temptable
            while (date<=toDate)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO #TempTable VALUES('"+hotelId+"','"+date.ToString("dd/MMMM/yyyy")+"')", sqlConnection);                
                cmd.ExecuteNonQuery();
                date.AddDays(1);
            }
            //Query for selecting the room details by date
            SqlCommand sqlCommand = new SqlCommand("SELECT SearchDate, Rooms.RoomNo, Rooms.RoomId, Bookings.BookingId, Guests.GuestName, RoomTypes.RoomTypeDesctiption, CASE WHEN RoomBookingId IS NOT NULL AND SearchDate NOT between CheckInDate and CheckOutDate THEN 'VACANT' WHEN RoomBookingId IS NOT NULL AND SearchDate = CheckINDate  THEN 'CHEKCEDIN' WHEN RoomBookingId IS NOT NULL AND SearchDate = CheckOutDate  THEN 'CHEKCEDOUT' WHEN RoomBookingId IS NOT NULL AND SearchDate between CheckInDate and CheckOutDate THEN 'STAY OVER' END AS RoomStatus, count(*) as Total FROM Hotels INNER JOIN #TempTable ON Hotels.HotelId=#TempTable.HotelId INNER JOIN Bookings on Bookings.HotelID = Hotels.HotelID LEFT OUTER JOIN RoomBookings  on RoomBookings.BookingID = Bookings.bookingID INNER JOIN Rooms On RoomBookings.RoomId = Rooms.RoomId INNER JOIN Guests ON Guests.GuestId = Bookings.GuestId INNER JOIN RoomTypes ON RoomTypes.RoomTypeId = Rooms.RoomTypeId WHERE  Hotels.HotelID = @hotelId GROUP BY SearchDate, Rooms.RoomNo, Rooms.RoomId, Bookings.BookingId, Guests.GuestName, RoomTypes.RoomTypeDesctiption, CASE WHEN RoomBookingId IS NOT NULL AND SearchDate NOT between CheckInDate and CheckOutDate THEN 'VACANT' WHEN RoomBookingId IS NOT NULL AND SearchDate = CheckINDate  THEN 'CHEKCEDIN' WHEN RoomBookingId IS NOT NULL AND SearchDate = CheckOutDate  THEN 'CHEKCEDOUT' WHEN RoomBookingId IS NOT NULL AND SearchDate between CheckInDate and CheckOutDate THEN 'STAY OVER' END ", sqlConnection);
            //Adding Parameters            
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
                    room.GuestName = dr["GuestName"].ToString();
                    room.RoomStatus = dr["RoomStatus"].ToString();
                    roomPlanList.Add(room);
                }
            }

            
            return roomPlanList;
        }
    }
}