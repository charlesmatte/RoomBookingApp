using RoomBookingApp.Core.Domain;

namespace RoomBookingApp.Core.DataServices
{
    public interface IRoomBookingServices
    {
        void Save(RoomBooking roomBooking);
        IEnumerable<Room> GetAvailableRooms(DateTime date);
    }
}