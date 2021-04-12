namespace Service.Tests.Builders
{
    using FakeItEasy;
    using Service.Contracts;

    public class TicketManagerBuilder
    {
        private ILogger logger;
        private ITicketsRepository ticketsRepository;
        private IEmailService emailService;
        private ISystemTime systemTime;

        public TicketManagerBuilder()
        {
            this.logger = A.Fake<ILogger>();
            this.ticketsRepository = A.Fake<ITicketsRepository>();
            this.emailService = A.Fake<IEmailService>();
            this.systemTime = A.Fake<ISystemTime>();
        }

        public TicketManagerBuilder WithILogger(ILogger logger)
        {
            this.logger = logger;
            return this;
        }

        public TicketManagerBuilder WithITicketsRepository(ITicketsRepository ticketsRepository)
        {
            this.ticketsRepository = ticketsRepository;
            return this;
        }

        public TicketManagerBuilder WithIEmailService(IEmailService emailService)
        {
            this.emailService = emailService;
            return this;
        }

        public TicketManagerBuilder WithISystemTime(ISystemTime systemTime)
        {
            this.systemTime = systemTime;
            return this;
        }

        public TicketManager Build()
        {
            return new TicketManager(this.logger, this.ticketsRepository, this.emailService, this.systemTime);
        }
    }

}
