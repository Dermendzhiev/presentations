namespace Service.Tests._01_Fixture_Management
{
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using Service.Contracts;
    using Service.Models;
    using Service.Tests.Builders;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class DelegatedSetup
    {
        [Test]
        public async Task RaiseTicketAsync_WhenTicketRequestIsCreated_ShouldReturnId()
        {
            // Arrange
            int expectedId = 1;

            ITicketsRepository ticketsRepository = A.Fake<ITicketsRepository>();
            A.CallTo(() => ticketsRepository.CreateAsync(A<TicketRequest>.Ignored))
                    .Returns(expectedId);

            TicketManager sut = new TicketManagerBuilder()
                .WithITicketsRepository(ticketsRepository)
                .Build();

            TicketRequest request = new TicketRequestBuilder().Build();

            // Act
            int actualId = await sut.RaiseTicketAsync(request);

            // Assert
            actualId.Should().Be(expectedId);
        }

        [Test]
        public void RaiseTicketAsync_WithNullTicketRequest_ShouldThrowArgumentNullException()
        {
            // Arrange
            TicketManager sut = new TicketManagerBuilder().Build();

            // Act
            AsyncTestDelegate act = () => sut.RaiseTicketAsync(null);

            // Assert
            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(act);
            exception.ParamName.Should().NotBeNullOrEmpty();
        }
    }
}
