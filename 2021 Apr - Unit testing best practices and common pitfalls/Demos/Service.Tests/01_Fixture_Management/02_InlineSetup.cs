namespace Service.Tests._01_Fixture_Management
{
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using Service.Contracts;
    using Service.Models;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class InlineSetup
    {
        [Test]
        public async Task RaiseTicketAsync_WhenTicketRequestIsCreated_ShouldReturnId()
        {
            // Arrange
            int expectedId = 1;

            ILogger logger = A.Fake<ILogger>();
            IEmailService emailService = A.Fake<IEmailService>();
            ISystemTime systemTime = A.Fake<ISystemTime>();

            ITicketsRepository ticketsRepository = A.Fake<ITicketsRepository>();
            A.CallTo(() => ticketsRepository.CreateAsync(A<TicketRequest>.Ignored))
                    .Returns(expectedId);

            TicketManager sut = new TicketManager(logger, ticketsRepository, emailService, systemTime);

            TicketRequest request = new TicketRequest("foo", "foo", 1);

            // Act
            int actualId = await sut.RaiseTicketAsync(request);

            // Assert
            actualId.Should().Be(expectedId);
        }

        [Test]
        public void RaiseTicketAsync_WithNullTicketRequest_ShouldThrowArgumentNullException()
        {
            // Arrange
            TicketManager sut = new TicketManager(null, null, null, null);

            // Act
            AsyncTestDelegate act = () => sut.RaiseTicketAsync(null);

            // Assert
            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(act);
            exception.ParamName.Should().NotBeNullOrEmpty();

            //exception.ParamName.Should().Be("request"); // Avoid overspecification
        }
    }
}
