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
        List<HouseKeepingDTO> GetHouseKeepingDetailsByHotelId(int hotelId);

    }
    public class HotelService : IHotelService
    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }

        List<HouseKeepingDTO> houseKeepingDetails = new List<HouseKeepingDTO>();
        public List<HouseKeepingDTO> GetHouseKeepingDetailsByHotelId(int hotelId)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Establishing the connection
            sqlConnection.Open();
            

            //Query for fetching the rooms details by hotelId 
            SqlCommand sqlCommand = new SqlCommand("SELECT RoomNo,RoomStatus from Rooms WHERE HotelId='" + hotelId + "'", sqlConnection);

            //Excuting the query
            SqlDataReader dr = sqlCommand.ExecuteReader();
            if (dr.HasRows)
            {
                //Reading the data
                while (dr.Read())
                {
                    //Creating an object and storing the values
                    HouseKeepingDTO houseKeeping = new HouseKeepingDTO();
                    houseKeeping.RoomNo = dr["RoomNo"].ToString();
                    var roomStatus= (RoomStatus)Enum.Parse(typeof(RoomStatus), dr["RoomStatus"].ToString());
                    houseKeeping.RoomStatus = roomStatus.ToString();
                    houseKeepingDetails.Add(houseKeeping);
                }
            }

            return houseKeepingDetails;
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
