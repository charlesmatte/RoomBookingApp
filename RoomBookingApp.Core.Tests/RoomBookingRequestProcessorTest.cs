using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core.Tests;

public class RoomBookingRequestProcessorTest
{
    [Fact]
    public void Should_Return_Room_Booking_Response_With_Request_Values()
    {
        // Arrange
        var bookingRequest = new RoomBookingRequest
        {
            FullName = "Test Name",
            Email = "test@request.com",
            Date = new DateTime(2021, 10, 20),
        };
        var processor = new RoomBookingRequestProcessor();
        // Act
        RoomBookingResult result = processor.BookRoom(bookingRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bookingRequest.FullName, result.FullName);
        Assert.Equal(bookingRequest.Email, result.Email);
        Assert.Equal(bookingRequest.Date, result.Date);

        result.ShouldNotBeNull();
        result.FullName.ShouldBe(bookingRequest.FullName);
        result.Email.ShouldBe(bookingRequest.Email);
        result.Date.ShouldBe(bookingRequest.Date);
    }
}