namespace Authorization.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        // Only logged users can access "SendMessage"
        public void SendMessage(string message)
        {
        }

        [Authorize("DomainRestricted")]
        public void ViewUserHistory(string username)
        {
        }
    }
}
