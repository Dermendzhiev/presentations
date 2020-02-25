namespace WhiteBoard.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class DrawHub : Hub
    {
        public Task Draw(int prevX, int prevY, int currentX, int currentY, string color) => this.Clients.Others.SendAsync("draw", prevX, prevY, currentX, currentY, color);
    }
}
