

using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Domain
{
    public class RoomBooking : RoomBookingBase
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}