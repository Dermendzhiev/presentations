namespace Service.Tests
{
    using FakeItEasy;
    using FluentAssertions;
    using Service.Contracts;
    using Service.Models;
    using Service.Tests.Builders;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class TicketManagerTests
    {
        [Fact]
        public async Task RaiseTicketAsync_WithNullTicketRequest_ShouldThrowArgumentNullException()
        {
            // Arrange
            TicketManager sut = new TicketManagerBuilder().Build();

            // Act
            Func<Task> act = () => sut.RaiseTicketAsync(null);

            // Assert
            ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
            exception.ParamName.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task RaiseTicketAsync_WithValidTicketRequest_ShouldLogInformation()
        {
            // Arrange
            ILogger loggerMock = A.Fake<ILogger>();

            TicketRequest ticketRequest = new TicketRequestBuilder().Build();

            TicketManager sut = new TicketManagerBuilder()
                .WithILogger(loggerMock)
                .Build();

            // Act
            await sut.RaiseTicketAsync(ticketRequest);

            // Assert
            A.CallTo(
                () => loggerMock.Information(
                    A<string>.That.Matches(x => !string.IsNullOrEmpty(x)),
                    A<object[]>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task RaiseTicketAsync_WhenTicketRequestIsCreated_ShouldBeSetUtcNowToRaisedAt()
        {
            // Arrange
            DateTime utcNow = new DateTime(2021, 01, 01);

            ISystemTime systemTimeStub = A.Fake<ISystemTime>();
            A.CallTo(() => systemTimeStub.UtcNow)
                .Returns(utcNow);

            ITicketsRepository ticketsRepositoryMock = A.Fake<ITicketsRepository>();

            TicketRequest ticketRequest = new TicketRequestBuilder().Build();

            TicketManager sut = new TicketManagerBuilder()
                .WithISystemTime(systemTimeStub)
                .WithITicketsRepository(ticketsRepositoryMock)
                .Build();

            // Act
            await sut.RaiseTicketAsync(ticketRequest);

            // Assert
            A.CallTo(
                () => ticketsRepositoryMock.CreateAsync(
                    A<TicketRequest>.That.Matches(x => x.CreatedAt == utcNow))).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task RaiseTicketAsync_WithValidTicketRequest_ShouldBeCreatedInRepository()
        {
            // Arrange
            ITicketsRepository ticketsRepositoryMock = A.Fake<ITicketsRepository>();

            TicketRequest ticketRequest = new TicketRequestBuilder().Build();

            TicketManager sut = new TicketManagerBuilder()
                .WithITicketsRepository(ticketsRepositoryMock)
                .Build();

            // Act
            await sut.RaiseTicketAsync(ticketRequest);

            // Assert
            A.CallTo(
                () => ticketsRepositoryMock.CreateAsync(
                    A<TicketRequest>.That.Matches(x => x == ticketRequest))).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task RaiseTicketAsync_WithValidTicketRequest_ShouldSendEmail()
        {
            // Arrange
            IEmailService emailServiceMock = A.Fake<IEmailService>();

            TicketRequest ticketRequest = new TicketRequestBuilder().Build();

            TicketManager sut = new TicketManagerBuilder()
                .WithIEmailService(emailServiceMock)
                .Build();

            // Act
            await sut.RaiseTicketAsync(ticketRequest);

            // Assert
            A.CallTo(
                () => emailServiceMock.SendEmailAsync(
                    A<string>.That.Matches(htmlContent => !string.IsNullOrEmpty(htmlContent)),
                    A<string>.That.Matches(toEmail => !string.IsNullOrEmpty(toEmail))))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task RaiseTicketAsync_WhenTicketRequestIsCreated_ReturnTicketRequestId()
        {
            // Arrange
            int expectedId = 1;

            ITicketsRepository ticketsRepositoryStub = A.Fake<ITicketsRepository>();
            A.CallTo(() => ticketsRepositoryStub.CreateAsync(A<TicketRequest>.Ignored))
                    .Returns(expectedId);

            TicketManager sut = new TicketManagerBuilder()
                .WithITicketsRepository(ticketsRepositoryStub)
                .Build();

            TicketRequest request = new TicketRequestBuilder().Build();

            // Act
            int actualId = await sut.RaiseTicketAsync(request);

            // Assert
            actualId.Should().Be(expectedId);
        }
    }
}