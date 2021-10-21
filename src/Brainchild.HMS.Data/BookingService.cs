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
    public interface IBookingService
    {
        int CreateGuest(GuestDTO guest);
        int CreateBooking(int guestId, BookingDTO booking);
        GuestDTO FindGuestByPhoneNumber(string phoneNo);
        List<Room> GetAvailableRooms(BookingDTO booking);
        void AddRoomBooking(int bookingId, int roomId);
        BookingDTO GetBookingDetails(int bookingId, int hotelId, string roomNo);       
        void DoCheckIn(string roomNo,int hotelId);
        void GenerateBill(int roomId, int bookingId);

    }
    public class BookingService : IBookingService
    {
        private readonly string connectionString;
        public BookingService(string connection)
        {
            connectionString = connection;
        }
        public int CreateGuest(GuestDTO guest)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the conenction
            sqlConnection.Open();

            //Sql query for creating Guest
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Guests values(@GuestName,@GuestAddress,@GuestEmail,@GuestPhoneNo,@GuestCountry); SELECT SCOPE_IDENTITY()", sqlConnection);

            //Adding parameters 
            sqlCommand.Parameters.AddWithValue("@GuestName", guest.GuestName);
            sqlCommand.Parameters.AddWithValue("@GuestAddress", guest.GuestAddress);
            sqlCommand.Parameters.AddWithValue("@GuestEmail", guest.GuestEmail);
            sqlCommand.Parameters.AddWithValue("@GuestPhoneNo", guest.GuestPhoneNo);
            sqlCommand.Parameters.AddWithValue("@GuestCountry", guest.GuestCountry);

            //Executing the query and storing the guestId 
            int guestId = Convert.ToInt32(sqlCommand.ExecuteScalar());

            //Returning guestId
            return guestId;

        }
        public int CreateBooking(int guestId, BookingDTO booking)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for creating a booking
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Bookings(GuestId,BookingDate,NoOfAdults,NoOfAChildren,CheckInDate,CheckOutDate,Status,HotelId) values(@gustId,@bookingDate,@noOdAdult,@noOfChildren,@checkIn,@checkOut,1,@hotelId);  SELECT SCOPE_IDENTITY()", sqlConnection);

            //Adding the parameters
            sqlCommand.Parameters.AddWithValue("@gustId", guestId);
            sqlCommand.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@noOdAdult", booking.NoOfAdults);
            sqlCommand.Parameters.AddWithValue("@noOfChildren", booking.NoOfAChildren);
            sqlCommand.Parameters.AddWithValue("@checkIn", booking.CheckInDate.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@checkOut", booking.CheckOutDate.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@hotelId", booking.HotelId);

            //Executing the query and storing the bookingId
            int bookingId = Convert.ToInt32(sqlCommand.ExecuteScalar());

            //Returning the bookingId
            return bookingId;
        }
        public GuestDTO FindGuestByPhoneNumber(string phoneNo)
        {
            //creating a GuestDTO object
            GuestDTO gust = new GuestDTO();

            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for selecting the guest details by phone number
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Guests WHERE GuestPhoneNo='" + phoneNo + "'", sqlConnection);

            //executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();

            //checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    //storing the details to guest object
                    gust.GuestId = Convert.ToInt32(dr["GuestId"]);
                    gust.GuestName = dr["GuestName"].ToString();
                    gust.GuestPhoneNo = dr["GuestPhoneNo"].ToString();
                    gust.GuestAddress = dr["GuestAddress"].ToString();
                    gust.GuestEmail = dr["GuestEmail"].ToString();
                    gust.GuestCountry = dr["GuestCountry"].ToString();
                }
            }
            //returning the guest object
            return gust;
        }
        List<Room> availableRooms = new List<Room>();
        public List<Room> GetAvailableRooms(BookingDTO booking)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for selecting the available rooms in the hotel
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM (SELECT RoomId, RoomNo FROM Rooms where HotelId='" + booking.HotelId + "') as T1 EXCEPT SELECT Rooms.RoomId, Rooms.RoomNo FROM Bookings  INNER JOIN RoomBookings ON RoomBookings.bookingid = Bookings.BookingId  INNER JOIN Rooms ON Rooms.RoomId = RoomBookings.RoomId WHERE CheckInDate = '" + booking.CheckInDate.ToString("dd/MMMM/yyyy") + "' and CheckOutDate = '" + booking.CheckOutDate.ToString("dd/MMMM/yyyy") + "'and Bookings.HotelId = '" + booking.HotelId + "'", sqlConnection);

            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();

            //Checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    //Creating an object for Room
                    Room room = new Room();
                    //storing the values to room object
                    room.RoomId = Convert.ToInt32(dr["RoomId"]);
                    room.RoomNo = dr["RoomNo"].ToString();
                    //adding the room object to availableroom list
                    availableRooms.Add(room);
                }
            }
            //Returning the list of available rooms
            return availableRooms;
        }

        public void AddRoomBooking(int bookingId, int roomId)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for Add room booking
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO RoomBookings values('" + bookingId + "','" + roomId + "')", sqlConnection);

            //Executing the query
            sqlCommand.ExecuteNonQuery();

            //closing the connection
            sqlConnection.Close();
        }

        public BookingDTO GetBookingDetails(int bookingId, int hotelId, string roomNo)
        {
            BookingDTO booking = new BookingDTO();            
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for selecting the booking details
            SqlCommand sqlCommand = new SqlCommand("SELECT Bookings.BookingId,CheckInDate,IsCancelled,Rooms.RoomId,RoomStatus FROM Bookings INNER JOIN RoomBookings ON Bookings.BookingId = RoomBookings.BookingId INNER JOIN Rooms ON RoomBookings.RoomID = Rooms.RoomID WHERE Bookings.BookingId = @bookingId AND Bookings.HotelId = @hotelId AND Rooms.RoomNo=@roomNo", sqlConnection);

            //Adding the parameters
            sqlCommand.Parameters.AddWithValue("@bookingId",bookingId);
            sqlCommand.Parameters.AddWithValue("@hotelId", hotelId);
            sqlCommand.Parameters.AddWithValue("@roomNo", roomNo);
            //executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();

            //checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    //storing the details to booking object
                    booking.BookingId = Convert.ToInt32(dr["BookingId"]);
                    booking.CheckInDate = Convert.ToDateTime(dr["CheckInDate"]);
                    booking.IsCancelled = Convert.ToInt32(dr["IsCancelled"]);
                    booking.RoomId = Convert.ToInt32(dr["RoomId"]);
                    object roomStatus = (object)dr["RoomStatus"];                    
                    booking.RoomStatus = (RoomStatus)roomStatus;
                }
            }
            //returning the booking object
            return booking;

        }
        
        public void DoCheckIn(string roomNo,int hotelId)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for changing the status og Room table for the check-in
            SqlCommand sqlCommand = new SqlCommand("UPDATE Rooms SET RoomStatus=1 WHERE RoomNo='" + roomNo + "' AND HotelId='" + hotelId + "'", sqlConnection);

            //Executing the query
            sqlCommand.ExecuteNonQuery();

            //Closing the connection 
            sqlConnection.Close();
        }

        public void GenerateBill(int roomId, int bookingId)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //query for generating bill
            SqlCommand command = new SqlCommand("INSERT INTO Billings(BillingDate,BookingId,RoomId) VALUES (@billDate,@bookingId,@roomId)", sqlConnection);
            
            //Adding the parameters
            command.Parameters.AddWithValue("@billDate", DateTime.Now.ToString("dd/MMMM/yyyy"));
            command.Parameters.AddWithValue("@bookingId", bookingId);
            command.Parameters.AddWithValue("@roomId", roomId);
            
            //executed the query
            command.ExecuteNonQuery();
            
            //closing the connection
            sqlConnection.Close();

        }
    }
}

