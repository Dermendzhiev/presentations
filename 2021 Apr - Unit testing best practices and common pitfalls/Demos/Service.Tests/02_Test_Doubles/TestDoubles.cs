namespace Service.Tests._02_Test_Doubles
{
    using FluentAssertions;
    using Moq;
    using Service.Contracts;
    using Service.Models;
    using Service.Tests.Builders;
    using System.Threading.Tasks;
    using Xunit;

    public class TestDoubles
    {
        #region Using_Moq

        [Fact]
        public async Task RaiseTicketAsync_WithValidTicketRequest_ReturnTicketRequestId()
        {
            // Arrange
            int expectedId = 1;

            Mock<ITicketsRepository> ticketsRepositoryMock = new Mock<ITicketsRepository>();
            ticketsRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<TicketRequest>()))
                .ReturnsAsync(expectedId);

            TicketManager sut = new TicketManagerBuilder()
                .WithITicketsRepository(ticketsRepositoryMock.Object)
                .Build();

            TicketRequest request = new TicketRequestBuilder().Build();

            // Act
            int actualId = await sut.RaiseTicketAsync(request);

            // Assert
            actualId.Should().Be(expectedId);
        }

        [Fact]
        public async Task RaiseTicketAsync_WithValidTicketRequest_ShouldLogInformation()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();

            TicketRequest ticketRequest = new TicketRequestBuilder().Build();
            TicketManager sut = new TicketManagerBuilder()
                .WithILogger(loggerMock.Object)
                .Build();

            // Act
            await sut.RaiseTicketAsync(ticketRequest);

            // Assert
            loggerMock.Verify(t => t.Information(
                It.IsNotNull<string>(),
                It.IsAny<object[]>()), Times.Once);
        }

        #endregion
    }
}
