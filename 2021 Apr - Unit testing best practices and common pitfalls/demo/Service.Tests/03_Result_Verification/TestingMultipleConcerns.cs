namespace Service.Tests._03_Result_Verification
{
    using FakeItEasy;
    using FluentAssertions;
    using Service.Contracts;
    using Service.Models;
    using Service.Tests.Builders;
    using System.Threading.Tasks;
    using Xunit;

    public class TestingMultipleConcerns
    {
        // Before:
        [Fact]
        public async Task RaiseTicketAsync_WithValidTicketRequest_ShouldLogInformationAndReturnTicketRequestId()
        {
            // Arrange
            int expectedId = 1;

            ITicketsRepository ticketsRepository = A.Fake<ITicketsRepository>();
            A.CallTo(() => ticketsRepository.CreateAsync(A<TicketRequest>.Ignored))
                    .Returns(expectedId);

            ILogger logger = A.Fake<ILogger>();

            TicketManager sut = new TicketManagerBuilder()
                .WithITicketsRepository(ticketsRepository)
                .WithILogger(logger)
                .Build();

            TicketRequest request = new TicketRequestBuilder().Build();

            // Act
            int actualId = await sut.RaiseTicketAsync(request);

            // Assert
            actualId.Should().Be(expectedId);
            A.CallTo(() => logger.Information(A<string>.Ignored, A<object[]>.Ignored)).MustHaveHappened();
        }

        // After:
        //[Fact]
        //public async Task RaiseTicketAsync_WithValidTicketRequest_ShouldLogInformation()
        //{
        //    // Arrange
        //    var loggerMock = A.Fake<ILogger>();

        //    var ticketRequest = new TicketRequestBuilder().Build();

        //    var sut = new TicketManagerBuilder()
        //        .WithILogger(loggerMock)
        //        .Build();

        //    // Act
        //    await sut.RaiseTicketAsync(ticketRequest);

        //    // Assert
        //    A.CallTo(
        //        () => loggerMock.Information(
        //            A<string>.That.Matches(x => !string.IsNullOrEmpty(x)),
        //            A<object[]>.Ignored)).MustHaveHappenedOnceExactly();
        //}

        //[Fact]
        //public async Task RaiseTicketAsync_WithValidTicketRequest_ReturnTicketRequestId()
        //{
        //    // Arrange
        //    int expectedId = 1;

        //    var ticketsRepository = A.Fake<ITicketsRepository>();
        //    A.CallTo(() => ticketsRepository.CreateAsync(A<TicketRequest>.Ignored))
        //            .Returns(expectedId);

        //    var sut = new TicketManagerBuilder()
        //        .WithITicketsRepository(ticketsRepository)
        //        .Build();

        //    var request = new TicketRequestBuilder().Build();

        //    // Act
        //    var actualId = await sut.RaiseTicketAsync(request);

        //    // Assert
        //    actualId.Should().Be(expectedId);
        //}
    }
}
