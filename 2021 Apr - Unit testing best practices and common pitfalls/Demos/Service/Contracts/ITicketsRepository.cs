namespace Service.Contracts
{
    using Service.Models;
    using System.Threading.Tasks;

    public interface ITicketsRepository
    {
        Task<int> CreateAsync(TicketRequest order);
    }
}
