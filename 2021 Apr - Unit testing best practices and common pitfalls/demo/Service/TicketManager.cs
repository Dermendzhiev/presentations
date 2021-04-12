/// <summary>
/// The use case below does NOT follow best practices. Is it only used to enable unit test demos.
/// </summary>

namespace Service
{
    using Service.Contracts;
    using Service.Models;
    using System;
    using System.Threading.Tasks;

    public class TicketManager
    {
        private readonly ILogger logger;
        private readonly ITicketsRepository ticketsRepository;
        private readonly IEmailService emailService;
        private readonly ISystemTime systemTime;

        public TicketManager(ILogger logger, ITicketsRepository ticketsRepository, IEmailService emailService, ISystemTime systemTime)
        {
            this.logger = logger;
            this.ticketsRepository = ticketsRepository;
            this.emailService = emailService;
            this.systemTime = systemTime;
        }

        public async Task<int> RaiseTicketAsync(TicketRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            this.logger.Information("Processing {@ticketRequest}", request);

            request.SetCreatedAt(DateTime.UtcNow); // this.systemTime.UtcNow
            int ticketRequestId = await this.ticketsRepository.CreateAsync(request);

            await this.emailService.SendEmailAsync($"<h1>New ticket created. ID: {ticketRequestId}", "support@gmail.com");

            return ticketRequestId;
        }
    }
}
