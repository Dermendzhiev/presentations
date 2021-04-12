namespace Service.Contracts
{
    using System;

    public interface ISystemTime
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}
