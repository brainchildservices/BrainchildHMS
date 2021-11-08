﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Brainchild.HMS.Data.DTOs;
using Brainchild.HMS.Core.Models;
using System.Text.Json.Serialization;

namespace Brainchild.HMS.Data
{
    public interface IHotelService
    {
        List<Room> GetAvailableRoomList(int hotelId, DateTime checkinDate, DateTime checkoutDate, string roomType, string roomStatus);
    }
    public class HotelService:IHotelService
    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }
        List<Room> availableRoomList = new List<Room>();
        public List<Room> GetAvailableRoomList(int hotelId, DateTime checkinDate, DateTime checkoutDate, string roomType, string roomStatus)
        {            
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();

            int status= (int)(RoomStatus)Enum.Parse(typeof(RoomStatus), roomStatus);

            //SQL query for fetching the booking details
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM (SELECT  RoomId, RoomNo, RoomRate,RoomStatus,RoomTypeDesctiption, Rooms.RoomTypeId, Rooms.HotelId, TenantID,OwnerName,RegistrationNo,HotelEmail,HotelPhone,Place,GSTNo FROM Rooms INNER JOIN RoomTypes ON RoomTypes.RoomTypeId = Rooms.RoomTypeId INNER JOIN Hotels On RoomTypes.HotelId=Hotels.HotelID WHERE RoomTypes.RoomTypeDesctiption = '" + roomType+"' AND Rooms.RoomStatus='"+ status + "' AND Rooms.HotelId = '"+hotelId+ "') as T1 EXCEPT SELECT Rooms.RoomId, Rooms.RoomNo, RoomTypes.RoomRate,Rooms.RoomStatus,RoomTypes.RoomTypeDesctiption, Rooms.RoomTypeId, Rooms.HotelId, TenantID,OwnerName,RegistrationNo,HotelEmail,HotelPhone,Place,GSTNo FROM Bookings  INNER JOIN RoomBookings ON RoomBookings.bookingid = Bookings.BookingId INNER JOIN Rooms ON Rooms.RoomId = RoomBookings.RoomId INNER JOIN RoomTypes ON RoomTypes.RoomTypeId = Rooms.RoomTypeId INNER JOIN Hotels On RoomTypes.HotelId=Hotels.HotelID WHERE CheckInDate = '" + checkinDate.ToString("dd/MMMM/yyyy")+"' AND CheckOutDate = '"+checkoutDate.ToString("dd/MMMM/yyyy")+"' AND Bookings.HotelId = '"+hotelId+"'", sqlConnection);
           
            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    Room rooms = new Room();
                    RoomType roomTypes = new RoomType();
                    Hotel hotels = new Hotel();
                    //Storing the data to hotel object
                    
                    hotels.HotelID = Convert.ToInt32(dr["HotelId"]);
                    hotels.TenantID = Convert.ToInt32(dr["TenantID"]);
                    hotels.OwnerName = dr["OwnerName"].ToString();
                    hotels.RegistrationNo = dr["RegistrationNo"].ToString();
                    hotels.HotelEmail = dr["HotelEmail"].ToString();
                    hotels.HotelPhone = dr["HotelPhone"].ToString();
                    hotels.Place = dr["Place"].ToString();
                    hotels.GSTNo = dr["GSTNo"].ToString();

                    //storing the data to roomtype object
                    roomTypes.RoomTypeId = Convert.ToInt32(dr["RoomTypeId"]);
                    roomTypes.RoomTypeDesctiption = dr["RoomTypeDesctiption"].ToString();
                    roomTypes.RoomRate =(float)(dr["RoomRate"]);
                    roomTypes.Hotel = hotels;

                    //storing the data to rooms object
                    rooms.RoomId = Convert.ToInt32(dr["RoomId"]);
                    rooms.RoomNo = dr["RoomNo"].ToString();     
                    object roomsStatus = (object)dr["RoomStatus"];                   
                    rooms.RoomStatus = (RoomStatus)roomsStatus;    
                    rooms.RoomType = roomTypes;
                    rooms.Hotel = hotels;
                    availableRoomList.Add(rooms);
                }
            }
            return availableRoomList;
        }
    }
}
