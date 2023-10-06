
using System.Data.Common;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors;

public class RoomBookingRequestProcessor
{
    private readonly IRoomBookingServices _roomBookingServices;
    public RoomBookingRequestProcessor(IRoomBookingServices roomBookingServices)
    {
        _roomBookingServices = roomBookingServices;
    }

    public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
    {
        if (bookingRequest is null)
        {
            throw new ArgumentNullException(nameof(bookingRequest));
        }
        var availableRooms = _roomBookingServices.GetAvailableRooms(bookingRequest.Date);
        var result = CreateRoomBooking<RoomBookingResult>(bookingRequest);
        if (availableRooms.Any())
        {
            var room = availableRooms.First();
            var roomBooking = CreateRoomBooking<RoomBooking>(bookingRequest);
            roomBooking.RoomId = room.Id;
            _roomBookingServices.Save(roomBooking);
            result.Flag = Enums.BookingResultFlag.Success;
        }
        else
        {
            result.Flag = Enums.BookingResultFlag.Failure;
        }

        return result;
    }

    private static TRoomBooking CreateRoomBooking<TRoomBooking>(RoomBookingRequest bookingRequest, Room room = null)
        where TRoomBooking : RoomBookingBase, new()
    {
        return new TRoomBooking
        {
            Id = new Random().Next(100),
            FullName = bookingRequest.FullName,
            Email = bookingRequest.Email,
            Date = bookingRequest.Date
        };
    }
}