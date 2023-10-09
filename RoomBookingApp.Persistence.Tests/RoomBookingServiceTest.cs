using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest
    {
        [Fact]
        public void Should_Return_Available_Rooms()
        {
            // Arrange
            var date = new DateTime(2021, 06, 09);
            var options = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("AvailableRoomTest")
                .Options;
            using var context = new RoomBookingAppDbContext(options);
            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });
            context.Add(new RoomBooking { Id = 1, RoomId = 1, Date = date, FullName = "Test", Email = "test@test.com" });
            context.Add(new RoomBooking { Id = 2, RoomId = 2, Date = date.AddDays(-1), FullName = "Test", Email = "test@test.com" });
            context.SaveChanges();
            var service = new RoomBookingService(context);

            // Act
            var availableRooms = service.GetAvailableRooms(date);

            // Assert
            Assert.Equal(2, availableRooms.Count());
            Assert.DoesNotContain(availableRooms, r => r.Id == 1);
            Assert.Contains(availableRooms, r => r.Id == 2);
            Assert.Contains(availableRooms, r => r.Id == 3);
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("SaveRoomBookingTest")
                .Options;

            using var context = new RoomBookingAppDbContext(dbOptions);
            var service = new RoomBookingService(context);
            var roomBooking = new RoomBooking
            {
                Id = 1,
                RoomId = 1,
                Date = new DateTime(2021, 06, 09),
                FullName = "Test",
                Email = "test@test.com"
            };

            // Act
            service.Save(roomBooking);

            // Assert
            var savedRoomBooking = context.RoomBookings.ToList();
            Assert.Single(savedRoomBooking);
            Assert.Equal(roomBooking.Id, savedRoomBooking[0].Id);
            Assert.Equal(roomBooking.RoomId, savedRoomBooking[0].RoomId);
            Assert.Equal(roomBooking.Date, savedRoomBooking[0].Date);
        }
    }
}