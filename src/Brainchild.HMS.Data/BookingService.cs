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
        int GetBookingId(string roomNo, int hotelId, int bookingId);
        void DoCheckIn(string roomNo);
        int GetRoomBookingCountByBookingId(int bookingId);
        void ChangeBookingStatus(int bookingId);
        void GenerateBill(string roomNo, int bookingId);

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
            SqlCommand sqlCommand = new SqlCommand("insert into Guests values(@GuestName,@GuestAddress,@GuestEmail,@GuestPhoneNo,@GuestCountry); SELECT SCOPE_IDENTITY()", sqlConnection);
            
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
            SqlCommand sqlCommand = new SqlCommand("insert into Bookings(GuestId,BookingDate,NoOfAdults,NoOfAChildren,CheckInDate,CheckOutDate,Status,HotelId) values(@gustid,@bookingDate,@NoOdAdult,@NoOfChildren,@checkin,@checkout,1,@hotelid);  SELECT SCOPE_IDENTITY()", sqlConnection);
            
            //Adding the parameters
            sqlCommand.Parameters.AddWithValue("@gustid", guestId);
            sqlCommand.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@NoOdAdult", booking.NoOfAdults);
            sqlCommand.Parameters.AddWithValue("@NoOfChildren", booking.NoOfAChildren);
            sqlCommand.Parameters.AddWithValue("@checkin", booking.CheckInDate.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@checkout", booking.CheckOutDate.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@hotelid", booking.HotelId);

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
            SqlCommand sqlCommand = new SqlCommand("select * from Guests where GuestPhoneNo='" + phoneNo + "'", sqlConnection);

            //executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();

            //checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    //soting the details to guest object
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
            SqlCommand sqlCommand = new SqlCommand("select * from (select RoomId, RoomNo from Rooms where HotelId='" + booking.HotelId + "') as T1 except select Rooms.RoomId, Rooms.RoomNo from Bookings  inner join RoomBookings on RoomBookings.bookingid = Bookings.BookingId  inner join Rooms on Rooms.RoomId = RoomBookings.RoomId where CheckInDate = '" + booking.CheckInDate.ToString("dd/MMMM/yyyy") + "' and CheckOutDate = '" + booking.CheckOutDate.ToString("dd/MMMM/yyyy") + "'and Bookings.HotelId = '" + booking.HotelId + "'", sqlConnection);
           
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
            SqlCommand cmd = new SqlCommand("insert into RoomBookings values('" + bookingId + "','" + roomId + "')", sqlConnection);

            //Executing the query
            cmd.ExecuteNonQuery();

            //closing the connection
            sqlConnection.Close();
        }
        public int GetBookingId(string roomNo, int hotelId, int bookingId)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for fetching the booking id
            SqlCommand sqlCommand = new SqlCommand("select * from Bookings inner join RoomBookings on RoomBookings.BookingId = Bookings.BookingId inner join Rooms on Rooms.RoomId = RoomBookings.RoomId inner join Guests on Guests.GuestId = Bookings.GuestId where Rooms.RoomNo = @roomNo and Bookings.HotelId = @hotelId and Bookings.BookingId = @bookingId and Bookings.Status=0", sqlConnection);
            
            //Adding patameters
            sqlCommand.Parameters.AddWithValue("@roomNo", roomNo);
            sqlCommand.Parameters.AddWithValue("@hotelId", hotelId);
            sqlCommand.Parameters.AddWithValue("@bookingId", bookingId);

            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();

            //Checking the object having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    //returning the bookingId
                    return Convert.ToInt32(dr["BookingId"]);
                }
            }
            return 0;
        }
        public void DoCheckIn(string roomNo)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for changing the status og Room table for the check-in
            SqlCommand sqlCommand = new SqlCommand("update Rooms set RoomStatus=1 where RoomNo='" + roomNo + "'", sqlConnection);
            
            //Executing the query
            sqlCommand.ExecuteNonQuery();
        }

        public int GetRoomBookingCountByBookingId(int bookingId)
        {
            int count = 0;
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Query for selecting  the roombooking count by bookingId
            SqlCommand sqlCommand = new SqlCommand("select * from Bookings inner join RoomBookings on Bookings.BookingId = RoomBookings.BookingId inner join Rooms on RoomBookings.RoomId = Rooms.RoomId where Rooms.RoomStatus = 0 and Bookings.BookingId ='" + bookingId + "'", sqlConnection);
            
            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();

            //Checking the object dr having data
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    //incrementing the counter variable
                    count++;
                }
            }

            //returning the count
            return count;
        }
        public void ChangeBookingStatus(int bookingId)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for changing the booking status
            SqlCommand sqlCommand = new SqlCommand("update Bookings set Status=1 where BookingId='" + bookingId + "'", sqlConnection);

            //Execute the query
            sqlCommand.ExecuteNonQuery();

            //Closing the connection
            sqlConnection.Close();
        }
        public void GenerateBill(string roomNo, int bookingId)
        {
            //Creating connection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Opening the connection
            sqlConnection.Open();

            //Sql query for selecting the roomid
            SqlCommand sqlCommand = new SqlCommand("select * from Rooms where RoomNo='" + roomNo + "'", sqlConnection);
            //executing the query
            SqlDataReader dr = sqlCommand.ExecuteReader();
            //initialized a variable roomId
            int roomId=0;
            //checking the objec dr havong rows
            if (dr.HasRows)
            {
                //reading the data
                while (dr.Read())
                {
                    //storing the value to roomId
                    roomId = Convert.ToInt32(dr["RoomId"]);
                }
            }
            //closing the connection
            sqlConnection.Close();
            //opening the connection
            sqlConnection.Open();

            //checking the roomid is null or not
            if (roomId != 0)
            {
                //query for generating bill
                SqlCommand command = new SqlCommand("insert into Billings(BillingDate,BookingId,RoomId) values(@billDate,@bookingId,@roomId)", sqlConnection);
                //Adding the parameters
                command.Parameters.AddWithValue("@billDate", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@bookingId", bookingId);
                command.Parameters.AddWithValue("@roomId", roomId);
                //executed the query
                command.ExecuteNonQuery();
                //closing the connection
                sqlConnection.Close();
            }
        }
    }
}

