using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core.Tests;

public class RoomBookingRequestProcessorTest
{
    private RoomBookingRequestProcessor _processor;
    private RoomBookingRequest _request;
    private Mock<IRoomBookingServices> _roomBookingServiceMock;
    private List<Room> _availableRooms;

    public RoomBookingRequestProcessorTest()
    {
        _request = new RoomBookingRequest
        {
            FullName = "Test Name",
            Email = "test@request.com",
            Date = new DateTime(2021, 10, 20),
        };
        _availableRooms = new List<Room>
        {
            new Room  () { Id = 1}
        };
        _roomBookingServiceMock = new Mock<IRoomBookingServices>();
        _roomBookingServiceMock.Setup(x => x.GetAvailableRooms(_request.Date))
            .Returns(_availableRooms);
        _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
    }

    [Fact]
    public void Should_Return_Room_Booking_Response_With_Request_Values()
    {
        // Arrange
        // Act
        RoomBookingResult result = _processor.BookRoom(_request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_request.FullName, result.FullName);
        Assert.Equal(_request.Email, result.Email);
        Assert.Equal(_request.Date, result.Date);

        result.ShouldNotBeNull();
        result.FullName.ShouldBe(_request.FullName);
        result.Email.ShouldBe(_request.Email);
        result.Date.ShouldBe(_request.Date);
    }

    [Fact]
    public void Should_Throw_Exception_For_Null_Request()
    {
        // Arrange
        // Act
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookRoom(null));
        exception.ParamName.ShouldBe("bookingRequest");
    }

    [Fact]
    public void Should_Save_Room_Booking_Request()
    {
        // Arrange
        RoomBooking savedBooking = null;
        _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
            .Callback<RoomBooking>(r => savedBooking = r);

        // Act
        _processor.BookRoom(_request);

        // Assert
        _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Once);
        savedBooking.ShouldNotBeNull();
        savedBooking.FullName.ShouldBe(_request.FullName);
        savedBooking.Email.ShouldBe(_request.Email);
        savedBooking.Date.ShouldBe(_request.Date);
        savedBooking.RoomId.ShouldBe(_availableRooms.First().Id);
    }

    [Fact]
    public void Should_Not_Save_Room_Booking_Request_If_None_Available()
    {
        // Arrange
        _availableRooms.Clear();

        // Act
        _processor.BookRoom(_request);

        // Assert
        _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Never);
    }

    [Theory]
    [InlineData(BookingResultFlag.Failure, false)]
    [InlineData(BookingResultFlag.Success, true)]
    public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResultFlag bookingSuccessFlag, bool isAvailable)
    {
        // Arrange
        if (!isAvailable)
        {
            _availableRooms.Clear();
        }
        // Act
        var result = _processor.BookRoom(_request);

        // Assert
        bookingSuccessFlag.ShouldBe(result.Flag);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(null, false)]
    public void Should_Return_RoomBookingId_In_Result(int? roomBookingId, bool isAvailable)
    {
        if (!isAvailable)
        {
            _availableRooms.Clear();
        }
        else
        {
            _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(r => r.Id = roomBookingId.Value);
        }

        var result = _processor.BookRoom(_request);
        result.Id.ShouldBe(roomBookingId);

    }
}
