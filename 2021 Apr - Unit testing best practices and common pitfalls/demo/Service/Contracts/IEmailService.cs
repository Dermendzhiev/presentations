namespace Service.Contracts
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendEmailAsync(string htmlContent, string toEmail);
    }
}
