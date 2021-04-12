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
    public class ImplicitSetup
    {
        private TicketManager sut; // sut = "system under test"
        private static readonly int createdTicketRequestId = 1;

        [SetUp]
        public void Init()
        {
            ILogger logger = A.Fake<ILogger>();

            ITicketsRepository ticketsRepository = A.Fake<ITicketsRepository>();
            A.CallTo(() => ticketsRepository.CreateAsync(A<TicketRequest>.Ignored))
                .Returns(createdTicketRequestId);

            IEmailService emailService = A.Fake<IEmailService>();

            ISystemTime systemTime = A.Fake<ISystemTime>();

            this.sut = new TicketManager(logger, ticketsRepository, emailService, systemTime);
        }

        [Test]
        public async Task RaiseTicketAsync_WhenTicketRequestIsCreated_ShouldReturnId()
        {
            // Arrange
            TicketRequest request = new TicketRequest("foo", "foo", 1);

            // Act
            int actualId = await this.sut.RaiseTicketAsync(request);

            // Assert
            actualId.Should().Be(createdTicketRequestId);
        }

        // The following test method, doesn't require any of the dependencies provided in the `Init` method
        [Test]
        public void RaiseTicketAsync_WithNullTicketRequest_ShouldThrowArgumentNullException()
        {
            // Act
            AsyncTestDelegate act = () => this.sut.RaiseTicketAsync(null);

            // Assert
            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(act);
            exception.ParamName.Should().NotBeNullOrEmpty();
        }
    }
}
