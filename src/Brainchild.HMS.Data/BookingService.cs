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
        void CancelBooking(int bookingId);
        void AddCancelNotes(int bookingId, string noteDescription);
        void DeleteRoomBookings(int bookingId);

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
            //Creating an sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //opening the connection
            sqlConnection.Open();

            //query for insert the Guest details
            SqlCommand sqlCommand = new SqlCommand("insert into Guests values(@GuestName,@GuestAddress,@GuestEmail,@GuestPhoneNo,@GuestCountry); SELECT SCOPE_IDENTITY()", sqlConnection);
            
            //Adding the parameters
            sqlCommand.Parameters.AddWithValue("@GuestName", guest.GuestName);
            sqlCommand.Parameters.AddWithValue("@GuestAddress", guest.GuestAddress);
            sqlCommand.Parameters.AddWithValue("@GuestEmail", guest.GuestEmail);
            sqlCommand.Parameters.AddWithValue("@GuestPhoneNo", guest.GuestPhoneNo);
            sqlCommand.Parameters.AddWithValue("@GuestCountry", guest.GuestCountry);   
            
            //Executed the query and stored the guestid
            int guestId = Convert.ToInt32(sqlCommand.ExecuteScalar());

            //Returning the guestId
            return guestId;

        }
        public int CreateBooking(int guestId, BookingDTO booking)
        {

            //creating an sqlconnection object.
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //opening the connection.
            sqlConnection.Open();

            //Query for inserting the values for booking
            SqlCommand sqlCommand = new SqlCommand("insert into Bookings(GuestId,BookingDate,NoOfAdults,NoOfAChildren,CheckInDate,CheckOutDate,Status,HotelId) values(@gustid,@bookingDate,@NoOdAdult,@NoOfChildren,@checkin,@checkout,1,@hotelid);  SELECT SCOPE_IDENTITY()", sqlConnection);
            
            //Adding the parameters.
            sqlCommand.Parameters.AddWithValue("@gustid", guestId);
            sqlCommand.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@NoOdAdult", booking.NoOfAdults);
            sqlCommand.Parameters.AddWithValue("@NoOfChildren", booking.NoOfChildren);
            sqlCommand.Parameters.AddWithValue("@checkin", booking.CheckInDate.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@checkout", booking.CheckOutDate.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@hotelid", booking.HotelId);

            //Executed the query and stored the bookingID
            int bookingId = Convert.ToInt32(sqlCommand.ExecuteScalar());

            //Returning the bookingId
            return bookingId;
        }
        public GuestDTO FindGuestByPhoneNumber(string phoneNo)
        {
            //creating an object for GuestDTO
            GuestDTO gust = new GuestDTO();

            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Establishing the connection
            sqlConnection.Open();

            //Query for selecting the Guest details with the phone number
            SqlCommand sqlCommand = new SqlCommand("select * from Guests where GuestPhoneNo='" + phoneNo + "'", sqlConnection);

            //Executing the query and storing the data
            SqlDataReader dr = sqlCommand.ExecuteReader();

            //Checking the object dr having values or not
            if (dr.HasRows)
            {
                //Reading the data row by row
                while (dr.Read())
                {
                    //storing the data to the object guest
                    gust.GuestId = Convert.ToInt32(dr["GuestId"]);
                    gust.GuestName = dr["GuestName"].ToString();
                    gust.GuestPhoneNo = dr["GuestPhoneNo"].ToString();
                    gust.GuestAddress = dr["GuestAddress"].ToString();
                    gust.GuestEmail = dr["GuestEmail"].ToString();
                    gust.GuestCountry = dr["GuestCountry"].ToString();
                }
            }

            //returning the object guest
            return gust;
        }
        List<Room> availableRooms = new List<Room>();
        public List<Room> GetAvailableRooms(BookingDTO booking)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Establishing the connection
            sqlConnection.Open();

            //Query for selecting the available rooms 
            SqlCommand sqlCommand = new SqlCommand("select * from (select RoomId, RoomNo from Rooms where HotelId='" + booking.HotelId + "') as T1 except select Rooms.RoomId, Rooms.RoomNo from Bookings  inner join RoomBookings on RoomBookings.bookingid = Bookings.BookingId  inner join Rooms on Rooms.RoomId = RoomBookings.RoomId where CheckInDate = '" + booking.CheckInDate.ToString("dd/MMMM/yyyy") + "' and CheckOutDate = '" + booking.CheckOutDate.ToString("dd/MMMM/yyyy") + "'and Bookings.HotelId = '" + booking.HotelId + "'", sqlConnection);
            
            //Excecuting the Query.
            SqlDataReader dr = sqlCommand.ExecuteReader();

            //Checking the object dr having values or not
            if (dr.HasRows)
            {

                //Reading the data row by row
                while (dr.Read())
                {
                    //Creating an object for Room
                    Room room = new Room();
                    //Storing the values to the object room
                    room.RoomId = Convert.ToInt32(dr["RoomId"]);
                    room.RoomNo = dr["RoomNo"].ToString();

                    //Adding the room value to the availablerooms list
                    availableRooms.Add(room);
                }
            }

            //Returning the available rooms list
            return availableRooms;
        }

        public void AddRoomBooking(int bookingId, int roomId)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Establishing the connection
            sqlConnection.Open();

            //Query for inserting the the values to RoomBooking table.
            SqlCommand sqlCommand = new SqlCommand("insert into RoomBookings values('" + bookingId + "','" + roomId + "')", sqlConnection);

            //Excuting the query
            sqlCommand.ExecuteNonQuery();

            //Closing the established connection
            sqlConnection.Close();            
        }

        
        public void CancelBooking(int bookingId)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Establishing the connection
            sqlConnection.Open();

            //Query for update the booking table for cancel a booking
            SqlCommand sqlCommand = new SqlCommand("UPDATE Bookings SET IsCancelled=@isCancelled, CancelledDate=@cancelledDate,Status=@status WHERE BookingId=@bookingId", sqlConnection);
            
            //Adding the parameters
            sqlCommand.Parameters.AddWithValue("@isCancelled", 1);
            sqlCommand.Parameters.AddWithValue("@cancelledDate", DateTime.Now.ToString("dd/MMMM/yyyy"));
            sqlCommand.Parameters.AddWithValue("@status", 3);
            sqlCommand.Parameters.AddWithValue("@bookingId", bookingId);
           
            //Executing the query
            sqlCommand.ExecuteNonQuery();

            //Closing the established connection
            sqlConnection.Close();
        }
        public void AddCancelNotes(int bookingId, string noteDescription)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Establishing the connection
            sqlConnection.Open();

            //Query for inserting the notes
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Notes VALUES(@noteDecription,@bookingId)", sqlConnection);

            //Adding the parameters
            sqlCommand.Parameters.AddWithValue("@noteDecription", noteDescription);
            sqlCommand.Parameters.AddWithValue("@bookingId", bookingId);

            //Executing the query
            sqlCommand.ExecuteNonQuery();

            //Closing the established connection
            sqlConnection.Close();
        }
       
        public void DeleteRoomBookings(int bookingId)
        {
            //creating an object for SqlConnection
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Establishing the connection
            sqlConnection.Open();

            //Query for deleting the roombookings
            SqlCommand sqlCommand = new SqlCommand("DELETE FROM RoomBookings WHERE BookingId='"+bookingId+"'", sqlConnection);

            //Executing the query
            sqlCommand.ExecuteNonQuery();

            //Closing the established connection
            sqlConnection.Close();
        }

    }
}
