using System;
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

        List<RoomPlanDTO> GetRoomPlan(DateTime fromDate, DateTime toDate, int hotelId);
        List<Charge> GetCharges(int bookingId, int roomId);
        CheckoutDetailsDTO GetCheckoutDetails(int bookingId, int roomId, int hotelId);
        void ChangeRoomStatus(int hotelId, string roomNo, string roomStatus);
        List<Room> GetHouseKeepingDetailsByHotelId(int hotelId);
        List<Room> GetAvailableRoomList(int hotelId, DateTime checkinDate, DateTime checkoutDate, string roomType, string roomStatus);

    }
    public class HotelService : IHotelService

    {
        private readonly string connectionString;
        public HotelService(string connection)
        {
            connectionString = connection;
        }


        List<Charge> charges = new List<Charge>();
        public List<Charge> GetCharges(int bookingId, int roomId)
        {
            CheckoutDetailsDTO checkoutDetails = new CheckoutDetailsDTO();
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();
            //SQL query for fetching the charges
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Charges INNER JOIN ChargeTypes ON ChargeTypes.ChargeTypeId = Charges.ChargeTypeId INNER JOIN Currencies ON Currencies.CurrencyId = Charges.CurrencyId INNER JOIN Rooms ON Rooms.RoomId = Charges.RoomId WHERE BookingId='" + bookingId + "' AND Rooms.RoomId='" + roomId + "'", sqlConnection);
            //Executing the Query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object dr having values
            if (dr.HasRows)
            {
                //Reading the data
                while (dr.Read())
                {

                    Charge charge = new Charge();
                    Room rooms = new Room();
                    ChargeType chargeType = new ChargeType();
                    Currency currency = new Currency();

                    rooms.RoomId = Convert.ToInt32(dr["RoomId"]);
                    rooms.RoomNo = dr["RoomNo"].ToString();
                    object roomsStatus = (object)dr["RoomStatus"];
                    rooms.RoomStatus = (RoomStatus)roomsStatus;

                    chargeType.ChargeTypeId = Convert.ToInt32(dr["ChargeTypeId"]);
                    chargeType.ChargeTypeDescription = dr["ChargeTypeDescription"].ToString();

                    currency.CurrencyId = Convert.ToInt32(dr["CurrencyId"]);
                    currency.CurrencyCountry = dr["CurrencyCountry"].ToString();
                    currency.CurrencyCode = dr["CurrencyCode"].ToString();
                    currency.CurrencyNumber = Convert.ToInt32(dr["CurrencyNumber"]);
                    currency.CurrencySymbol = dr["CurrencySymbol"].ToString();

                    charge.ChargeId = Convert.ToInt32(dr["ChargeId"]);
                    charge.Currency = currency;
                    charge.ChargeType = chargeType;
                    charge.Room = rooms;
                    charge.ChargeAmount = float.Parse(dr["ChargeAmount"].ToString());                  


                    charges.Add(charge);

                }
            }
            return charges;
        }
        public CheckoutDetailsDTO GetCheckoutDetails(int bookingId, int roomId, int hotelId)
        {
            CheckoutDetailsDTO checkoutDetails = new CheckoutDetailsDTO();
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();
            //SQL query for fetching the checout details
            SqlCommand sqlCommand = new SqlCommand("SELECT Bookings.BookingId, Guests.GuestName, Bookings.CheckInDate, Bookings.CheckOutDate, RoomBookings.RoomRate, Rooms.RoomNo FROM Bookings INNER JOIN RoomBookings ON Bookings.BookingId = RoomBookings.BookingId INNER JOIN Rooms ON Rooms.RoomId = RoomBookings.RoomId INNER JOIN Guests ON Guests.GuestId = Bookings.GuestId WHERE Bookings.BookingId = '" + bookingId + "' AND Bookings.HotelId = '" + hotelId + "' AND Rooms.RoomId = '" + roomId + "'", sqlConnection);
            //Executing the Query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //Checking the object dr having values

            if (dr.HasRows)
            {
                //Reading the data
                while (dr.Read())
                {

                    checkoutDetails.BookingId = Convert.ToInt32(dr["BookingId"]);
                    checkoutDetails.GuestName = dr["GuestName"].ToString();
                    checkoutDetails.CheckInDate = Convert.ToDateTime(dr["CheckInDate"]);
                    checkoutDetails.CheckOutDate = Convert.ToDateTime(dr["CheckOutDate"]);
                    checkoutDetails.RoomNo = dr["RoomNo"].ToString();
                    checkoutDetails.RoomRate = Convert.ToDouble(dr["RoomRate"]);
                }
            }
            return checkoutDetails;

        }
        List<Room> houseKeepingDetails = new List<Room>();
        public List<Room> GetHouseKeepingDetailsByHotelId(int hotelId)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            //Query for fetching the rooms details by hotelId 
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Rooms INNER JOIN RoomTypes ON RoomTypes.RoomtypeId = Rooms.RoomTypeId WHERE Rooms.HotelId = '" + hotelId + "'", sqlConnection);
            //Excuting the query
            SqlDataReader dr = sqlCommand.ExecuteReader();
            while (dr.Read())
            {
                //Creating an object and storing the values
                Room rooms = new Room();
                RoomType roomTypes = new RoomType();
                //storing the data to roomtype object
                roomTypes.RoomTypeId = Convert.ToInt32(dr["RoomTypeId"]);
                roomTypes.RoomTypeDesctiption = dr["RoomTypeDesctiption"].ToString();
                roomTypes.RoomRate = (float)(dr["RoomRate"]);


                //storing the data to rooms object
                rooms.RoomId = Convert.ToInt32(dr["RoomId"]);
                rooms.RoomNo = dr["RoomNo"].ToString();
                object roomsStatus = (object)dr["RoomStatus"];
                rooms.RoomStatus = (RoomStatus)roomsStatus;
                rooms.RoomType = roomTypes;

                houseKeepingDetails.Add(rooms);
            }
            return houseKeepingDetails;
        }
        public void ChangeRoomStatus(int hotelId, string roomNo, string roomStatus)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Establishing the connection
            sqlConnection.Open();

            //Get the value of the enum
            object roomStatusValue = Enum.Parse(typeof(RoomStatus), roomStatus);

            //Query for Changing the status 
            SqlCommand sqlCommand = new SqlCommand("UPDATE Rooms SET RoomStatus='" + (int)roomStatusValue + "' WHERE RoomNo='" + roomNo + "' AND HotelId='" + hotelId + "'", sqlConnection);

            //Excuting the query
            sqlCommand.ExecuteNonQuery();

            //Closing the established connection
            sqlConnection.Close();
        }
        List<Room> availableRoomList = new List<Room>();
        public List<Room> GetAvailableRoomList(int hotelId, DateTime checkinDate, DateTime checkoutDate, string roomType, string roomStatus)
        {
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Opening the connection
            sqlConnection.Open();
            int status = (int)(RoomStatus)Enum.Parse(typeof(RoomStatus), roomStatus);

            //SQL query for fetching the booking details
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM (SELECT  RoomId, RoomNo, RoomRate,RoomStatus,RoomTypeDesctiption, Rooms.RoomTypeId, Rooms.HotelId, TenantID,OwnerName,RegistrationNo,HotelEmail,HotelPhone,Place,GSTNo FROM Rooms INNER JOIN RoomTypes ON RoomTypes.RoomTypeId = Rooms.RoomTypeId INNER JOIN Hotels On RoomTypes.HotelId=Hotels.HotelID WHERE RoomTypes.RoomTypeDesctiption = '" + roomType + "' AND Rooms.RoomStatus='" + status + "' AND Rooms.HotelId = '" + hotelId + "') as T1 EXCEPT SELECT Rooms.RoomId, Rooms.RoomNo, RoomTypes.RoomRate,Rooms.RoomStatus,RoomTypes.RoomTypeDesctiption, Rooms.RoomTypeId, Rooms.HotelId, TenantID,OwnerName,RegistrationNo,HotelEmail,HotelPhone,Place,GSTNo FROM Bookings  INNER JOIN RoomBookings ON RoomBookings.bookingid = Bookings.BookingId INNER JOIN Rooms ON Rooms.RoomId = RoomBookings.RoomId INNER JOIN RoomTypes ON RoomTypes.RoomTypeId = Rooms.RoomTypeId INNER JOIN Hotels On RoomTypes.HotelId=Hotels.HotelID WHERE CheckInDate = '" + checkinDate.ToString("dd/MMMM/yyyy") + "' AND CheckOutDate = '" + checkoutDate.ToString("dd/MMMM/yyyy") + "' AND Bookings.HotelId = '" + hotelId + "'", sqlConnection);

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
                    roomTypes.RoomRate = (float)(dr["RoomRate"]);
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
            sqlCommand.Parameters.AddWithValue("@hotelId", hotelId);
            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    RoomPlanDTO room = new RoomPlanDTO();
                    room.FromDate = fromDate;
                    room.ToDate = toDate;
                    room.RoomId = Convert.ToInt32(dr["RoomId"]);
                    room.RoomNo = dr["RoomNo"].ToString();
                    room.RoomType = dr["RoomTypeDesctiption"].ToString();
                    room.SearchDate = Convert.ToDateTime(dr["SearchDate"]);
                    room.RoomStatus = dr["RoomStatus"].ToString();
                    if (room.RoomStatus == "VACANT")
                    {
                        room.BookingId = 0;
                        room.GuestName = null;
                    }
                    else
                    {
                        room.BookingId = Convert.ToInt32(dr["BookingId"]);
                        room.GuestName = dr["GuestName"].ToString();
                    }
                    roomPlanList.Add(room);
                }
            }
            return roomPlanList;
        }
    }
}

