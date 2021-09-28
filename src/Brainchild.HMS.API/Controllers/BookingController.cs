using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Brainchild.HMS.Core.Models;
using Brainchild.HMS.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Brainchild.HMS.API.DTOs;
using System.Data;
using System.Data.SqlClient;

namespace Brainchild.HMS.API.Controllers
{
    [Route("hms/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class BookingController : ControllerBase
    {
        private readonly BrainchildHMSDbContext _context;
        private readonly ILogger<BookingController> _logger;

        public BookingController(BrainchildHMSDbContext context, ILogger<BookingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Booking/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        string cs = "Data Source=SNEHA;Initial Catalog=BrainchildHMS;Integrated Security=True;";

        public void newbooking(int guestid, BookingDTO booking)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Bookings(GuestId,BookingDate,NoOfAdults,NoOfAChildren,CheckInDate,CheckOutDate,Status,HotelId) values(@gustid,@bookingDate,@NoOdAdult,@NoOfChildren,@checkin,@checkout,1,@hotelid)", con);
            cmd.Parameters.AddWithValue("@gustid", guestid);
            cmd.Parameters.AddWithValue("@bookingDate", DateTime.Now.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@NoOdAdult", booking.NoOfAdults);
            cmd.Parameters.AddWithValue("@NoOfChildren", booking.NoOfAChildren);
            cmd.Parameters.AddWithValue("@checkin", booking.CheckInDate.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@checkout", booking.CheckOutDate.ToString("dd/MMMM/yyyy"));
            cmd.Parameters.AddWithValue("@hotelid", booking.HotelId);
            cmd.ExecuteNonQuery();

        }
        public int chechGuest(string phone)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Guests where GuestPhoneNo='" + phone + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["GuestId"]);
                    return id;
                }
            }

            return 0;
        }
        public void insert_guest(GuestDTO guest)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Guests values(@GuestName,@GuestAddress,@GuestEmail,@GuestPhoneNo,@GuestCountry)", con);
            cmd.Parameters.AddWithValue("@GuestName", guest.GuestName);
            cmd.Parameters.AddWithValue("@GuestAddress", guest.GuestAddress);
            cmd.Parameters.AddWithValue("@GuestEmail", guest.GuestEmail);
            cmd.Parameters.AddWithValue("@GuestPhoneNo", guest.GuestPhoneNo);
            cmd.Parameters.AddWithValue("@GuestCountry", guest.GuestCountry);
            cmd.ExecuteNonQuery();
        }
        public int checkRoom(BookingDTO booking)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from (select RoomId, RoomNo from Rooms where HotelId='"+booking.HotelId+"') as T1 except select Rooms.RoomId, Rooms.RoomNo from Bookings  inner join RoomBookings on RoomBookings.bookingid = Bookings.BookingId  inner join Rooms on Rooms.RoomId = RoomBookings.RoomId where CheckInDate = '" + booking.CheckInDate.ToString("dd/MMMM/yyyy")+"' and CheckOutDate = '"+booking.CheckOutDate.ToString("dd/MMMM/yyyy")+"'and Bookings.HotelId = '"+booking.HotelId+"'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["RoomId"]);
                    return id;
                }
            }

            return 0;
        }
        public int get_bookingId()
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("select max(BookingId) from Bookings", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr[0]);
                    return id;
                }
            }

            return 0;
        }
        public void insert_RoomBooking(int bookingid,int roomid)
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into RoomBookings values('"+bookingid+"','"+roomid+"')", con);            
            cmd.ExecuteNonQuery();
        }
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO booking)
        {

            int guestid = chechGuest(booking.Guest.GuestPhoneNo.ToString());
            if (guestid == 0)
            {
                insert_guest(booking.Guest);
                guestid = chechGuest(booking.Guest.GuestPhoneNo);
                int roomid = checkRoom(booking);
                if (roomid == 0)
                {
                    //message there is no room available
                }
                else
                {
                    newbooking(guestid, booking);
                    int bid = get_bookingId();
                    insert_RoomBooking(bid, roomid);
                }
               
            }
            else
            {
                int roomid = checkRoom(booking);
                if (roomid == 0)
                {
                    //message there is no room available
                }
                else
                {
                    newbooking(guestid, booking);
                    int bid = get_bookingId();
                    insert_RoomBooking(bid, roomid);
                }
            }

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
