using Microsoft.AspNetCore.SignalR;

namespace PRN222_BL3_Project.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("User"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Users");
            }
            await base.OnConnectedAsync();
        }
    }
}
