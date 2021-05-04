using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Heartbeat.Hubs
{
    public class EkgHub : Hub
    {
        public const string HubUrl = "/cable"; 
        
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
    }
}