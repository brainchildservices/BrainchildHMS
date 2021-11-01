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
            SqlCommand sqlCommand = new SqlCommand("EXEC GetRoomPlanProcedure @fromDate,@toDate,@hotelId", sqlConnection);
            //Adding parameters
            sqlCommand.Parameters.AddWithValue("@fromDate", fromDate);
            sqlCommand.Parameters.AddWithValue("@toDate", toDate);
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
                    room.RoomType = dr["RoomTypeDesctiption"].ToString();
                    room.GuestName = dr["GuestName"].ToString();
                    room.RoomStatus = dr["RoomStatus"].ToString();
                    room.SearchDate = Convert.ToDateTime(dr["SearchDate"]);
                    roomPlanList.Add(room);
                }
            }

            
            return roomPlanList;
        }
    }
}