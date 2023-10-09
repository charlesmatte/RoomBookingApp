using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomBookingController : ControllerBase
    {
        private readonly IRoomBookingRequestProcessor _roomBookingRequestProcessor;

        public RoomBookingController(IRoomBookingRequestProcessor roomBookingRequestProcessor)
        {
            _roomBookingRequestProcessor = roomBookingRequestProcessor;
        }

        [HttpPost]
        public IActionResult BookRoom(RoomBookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _roomBookingRequestProcessor.BookRoom(request);

            return result.Flag switch
            {
                BookingResultFlag.Success => Ok(result),
                BookingResultFlag.Failure => BadRequest(result),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}