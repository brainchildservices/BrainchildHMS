using Microsoft.EntityFrameworkCore.Migrations;

namespace Brainchild.HMS.Data.Migrations
{
    public partial class GetRoomPlanByDatesProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcSql = @"CREATE  PROC GetRoomPlanByDatesProcedure(@fromDate Date, @toDate Date, @hotelId int) AS 
                            CREATE TABLE #TempTable(HotelId int, SearchDate date)

                            DECLARE @startDate AS DATE
                            DECLARE @endDate AS DATE
                            DECLARE @hotelsId AS int
                            SET @startDate=@fromDate
                            SET @endDate=@toDate
                            SET @hotelsId=@hotelId
                            
                            WHILE (@startDate <= @endDate)
                            BEGIN                                
	                            INSERT INTO #TempTable VALUES(@hotelsId,@startDate);               
	                            SET @startDate = DATEADD(DAY, 1, @endDate);
                            END

   
                            SELECT SearchDate ,Rooms.RoomNo,   Rooms.RoomId,   Bookings.BookingId,   Guests.GuestName, RoomTypes.RoomTypeDesctiption,   CASE	
                                WHEN RoomBookingId IS NOT NULL AND SearchDate NOT between CheckInDate and CheckOutDate THEN 'VACANT'
                                WHEN RoomBookingId IS NOT NULL AND SearchDate = CheckINDate  THEN 'CHEKCEDIN'
                                WHEN RoomBookingId IS NOT NULL AND SearchDate = CheckOutDate  THEN 'CHEKCEDOUT'
                                WHEN RoomBookingId IS NOT NULL AND SearchDate between CheckInDate and CheckOutDate THEN 'STAY OVER'    
                            END AS RoomStatus, count(*) as total
                            FROM 
                            Hotels INNER JOIN #TempTable ON Hotels.HotelId=#TempTable.HotelId
                            INNER JOIN Bookings on Bookings.HotelID = Hotels.HotelID
                            LEFT OUTER JOIN RoomBookings  on RoomBookings.BookingID = Bookings.bookingID
                            INNER JOIN Rooms On RoomBookings.RoomId = Rooms.RoomId
                            INNER JOIN Guests ON Guests.GuestId=Bookings.GuestId
                            INNER JOIN RoomTypes ON RoomTypes.RoomTypeId=Rooms.RoomTypeId
                            WHERE  Hotels.HotelID = @hotelsId
                            GROUP BY SearchDate, Rooms.RoomNo,   Rooms.RoomId,   Bookings.BookingId,   Guests.GuestName, RoomTypes.RoomTypeDesctiption,   CASE	
                                WHEN RoomBookingId IS NOT NULL AND SearchDate NOT between CheckInDate and CheckOutDate THEN 'VACANT'
                                WHEN RoomBookingId IS NOT NULL AND SearchDate = CheckINDate  THEN 'CHEKCEDIN'
                                WHEN RoomBookingId IS NOT NULL AND SearchDate = CheckOutDate  THEN 'CHEKCEDOUT'
                                WHEN RoomBookingId IS NOT NULL AND SearchDate between CheckInDate and CheckOutDate THEN 'STAY OVER'    
                            END 

                            ";
            migrationBuilder.Sql(createProcSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropProcSql = "DROP PROC GetRoomPlanByDatesProcedure";
            migrationBuilder.Sql(dropProcSql);
        }
    }
}
