namespace RoomBookingApp.Core.Models;

public abstract class RoomBookingBase
{
    public int? Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
}