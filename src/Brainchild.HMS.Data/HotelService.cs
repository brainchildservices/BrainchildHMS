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

        List<RoomDTO> GetAvailableRoomList(int hotelId, DateTime checkinDate, DateTime checkoutDate, string roomType, string roomStatus);
    }
    public class HotelService:IHotelService

    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }

        List<RoomDTO> availableRoomList = new List<RoomDTO>();
        public List<RoomDTO> GetAvailableRoomList(int hotelId, DateTime checkinDate, DateTime checkoutDate, string roomType, string roomStatus)
        {            
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();

            int status= (int)(RoomStatus)Enum.Parse(typeof(RoomStatus), roomStatus);

            //SQL query for fetching the booking details
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM (SELECT  RoomId, RoomNo, RoomRate,RoomStatus,RoomTypeDesctiption FROM Rooms INNER JOIN RoomTypes ON RoomTypes.RoomTypeId = Rooms.RoomTypeId WHERE RoomTypes.RoomTypeDesctiption = '" + roomType+"' AND Rooms.RoomStatus='"+ status + "' AND Rooms.HotelId = '"+hotelId+ "') as T1 EXCEPT SELECT Rooms.RoomId, Rooms.RoomNo, RoomTypes.RoomRate,Rooms.RoomStatus,RoomTypes.RoomTypeDesctiption FROM Bookings  INNER JOIN RoomBookings ON RoomBookings.bookingid = Bookings.BookingId INNER JOIN Rooms ON Rooms.RoomId = RoomBookings.RoomId INNER JOIN RoomTypes ON RoomTypes.RoomTypeId = Rooms.RoomTypeId WHERE CheckInDate = '" + checkinDate.ToString("dd/MMMM/yyyy")+"' AND CheckOutDate = '"+checkoutDate.ToString("dd/MMMM/yyyy")+"' AND Bookings.HotelId = '"+hotelId+"'", sqlConnection);
           
            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object having data

       
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {

                    RoomDTO rooms = new RoomDTO();
                    //Storing the data to room object
                    rooms.RoomId = Convert.ToInt32(dr["RoomId"]);
                    rooms.RoomNo = dr["RoomNo"].ToString();
                    rooms.RoomType = dr["RoomTypeDesctiption"].ToString();
                    rooms.RoomRate = Convert.ToDouble(dr["RoomRate"]);
                    var statusRoom= (RoomStatus)Enum.Parse(typeof(RoomStatus), dr["RoomStatus"].ToString());
                    rooms.RoomStatus = statusRoom.ToString();
                    availableRoomList.Add(rooms);
                }
            }
            return availableRoomList;
        }
    }
}

