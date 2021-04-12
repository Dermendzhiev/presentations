namespace Service.Tests.Builders
{
    using Service.Models;

    public class TicketRequestBuilder
    {
        private string title;
        private string description;
        private int createdByUserId;

        public TicketRequestBuilder()
        {
            this.title = "foo";
            this.description = "foo";
            this.createdByUserId = 1;
        }

        public TicketRequestBuilder WithTitle(string title)
        {
            this.title = title;
            return this;
        }

        public TicketRequestBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public TicketRequestBuilder WithCreatedByUserId(int createdByUserId)
        {
            this.createdByUserId = createdByUserId;
            return this;
        }

        public TicketRequest Build()
        {
            return new TicketRequest(this.title, this.description, this.createdByUserId);
        }
    }
}
