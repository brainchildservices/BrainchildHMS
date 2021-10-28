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
        void ChangeRoomStatus(int hotelId, string roomNo, string roomStatus);
    }
    public class HotelService : IHotelService
    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }
        public void ChangeRoomStatus(int hotelId, string roomNo,string roomStatus)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Establishing the connection
            sqlConnection.Open();
            
            //Get the value of the enum
            object roomStatusValue=Enum.Parse(typeof(RoomStatus), roomStatus);            

            //Query for Changing the status 
            SqlCommand sqlCommand = new SqlCommand("UPDATE Rooms SET RoomStatus='"+  (int)roomStatusValue + "' WHERE RoomNo='"+roomNo+"' AND HotelId='"+hotelId+"'", sqlConnection);

            //Excuting the query
            sqlCommand.ExecuteNonQuery();

            //Closing the established connection
            sqlConnection.Close();
        }
    }
}
