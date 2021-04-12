namespace Service.Models
{
    using System;

    public class TicketRequest
    {
        public TicketRequest(string title, string description, int createdByUserId)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            if (createdByUserId <= 0)
            {
                throw new ArgumentException(nameof(createdByUserId));
            }

            Title = title;
            Description = description;
            CreatedByUserId = createdByUserId;
        }

        public string Title { get; }

        public string Description { get; }

        public DateTime CreatedAt { get; private set; }

        public int CreatedByUserId { get; }

        public void SetCreatedAt(DateTime createdAt)
        {
            if (createdAt == default)
            {
                throw new ArgumentException(nameof(createdAt));
            }

            this.CreatedAt = createdAt;
        }
    }
}