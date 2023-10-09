using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.Api.Controllers;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Api.Tests
{
    public class RoomBookingControllerTests
    {
        private Mock<IRoomBookingRequestProcessor> _roomBookingProcessorMock;
        private RoomBookingController _controller;
        private RoomBookingRequest _request;
        private RoomBookingResult _result;

        public RoomBookingControllerTests()
        {
            _roomBookingProcessorMock = new Mock<IRoomBookingRequestProcessor>();
            _controller = new RoomBookingController(_roomBookingProcessorMock.Object);
            _request = new RoomBookingRequest();
            _result = new RoomBookingResult();

            _roomBookingProcessorMock.Setup(x => x.BookRoom(_request)).Returns(_result);
        }

        [Theory]
        [InlineData(1, true, typeof(OkObjectResult))]
        [InlineData(0, false, typeof(BadRequestObjectResult))]
        public void Should_Call_Booking_Method_When_Valid(int expectedMethodCalls, bool isModelValid, Type expectedActionResultType)
        {
            // Arrange
            if (!isModelValid)
            {
                _controller.ModelState.AddModelError("Key", "ErrorMessage");
            }

            // Act
            var result = _controller.BookRoom(_request);

            // Assert
            result.ShouldBeOfType(expectedActionResultType);
            _roomBookingProcessorMock.Verify(x => x.BookRoom(_request), Times.Exactly(expectedMethodCalls));

        }
    }
}