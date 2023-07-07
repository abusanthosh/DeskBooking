using DeskBooking.Core.Domain;
using DeskBooking.Core.Interfaces;
using DeskBooking.Core.Processor;
using Moq;

namespace DeskBooking.Test.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private DeskBookingRequest _request;
        private List<Desk> _availableDesks;
        private Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private Mock<IDeskRepository> _deskBookingMock;
        private DeskBookingRequestProcessor _processor;

        public DeskBookingRequestProcessorTests()
        {
            _request = new DeskBookingRequest
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "test@test.com",
                BookingDate = new DateTime(2023, 7, 7)
            };

            _availableDesks = new List<Desk> { new Desk() };

            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _deskBookingMock = new Mock<IDeskRepository>();
            _deskBookingMock.Setup(x => x.GetAvailableDesks(_request.BookingDate)).Returns(_availableDesks);

            _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object, _deskBookingMock.Object);
        }

        [Fact]
        public void ShouldReturnDeskBookingResultWithRequestValues()
        {
            //Act
            DeskBookingResult result = _processor.BookDesk(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_request.FirstName, result.FirstName);
        }

        [Fact]
        public void ShouldThrowExceptionWhenRequestIsEmpty()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));
            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            DeskBookingModel savedDeskBookingModel = null;
            _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBookingModel>()))
                .Callback<DeskBookingModel>(deskBooking => savedDeskBookingModel = deskBooking);

            _processor.BookDesk(_request);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBookingModel>()), Times.Once);

            Assert.NotNull(savedDeskBookingModel);
            Assert.Equal(_request.FirstName, savedDeskBookingModel.FirstName);
        }

        [Fact]
        public void ShouldNotSaveWhenDeskNotAvailable()
        {
            _availableDesks.Clear();

            _processor.BookDesk(_request);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBookingModel>()), Times.Never);


        }

    }
}
