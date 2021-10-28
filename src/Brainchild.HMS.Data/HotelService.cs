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
        List<Room> GetRoomPlan(DateTime fromDate, int hotelId);
    }
    public class HotelService : IHotelService
    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }
        List<Room> roomPlanList = new List<Room>();
        public List<Room> GetRoomPlan(DateTime fromDate, int hotelId)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Establishing the connection
            sqlConnection.Open();            
            //Query for selecting the room details by date
            SqlCommand sqlCommand = new SqlCommand("", sqlConnection);
            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    Room room = new Room();

                    roomPlanList.Add(room);
                }
            }

            
            return roomPlanList;
        }
    }
}