using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace BlazorApp1.Hubs;

public class ChatHub : Hub
{
    public async Task UpdateData()
    {
        await Clients.All.SendAsync("ReceiveMessage", GetData());
    }
    
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} вошел в чат");
        await base.OnConnectedAsync();

        Console.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
    }
    
    public async Task UpdateGroup(int groupId)
    {
        /*Group Clients.Client(Context.ConnectionId).
        var groups = await Groups.GetConnectionsAsync(userId);
            
        foreach (var group in groups)
        {
            await Groups.(group, userId);
        }
        Groups.RemoveFromGroupAsync(Context.ConnectionId);*/
        await Groups.AddToGroupAsync(Context.ConnectionId, "group_" + groupId);
        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAA" + groupId);
        //await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public static string GetData()
    {
        var r = new Random();
        return r.Next().ToString();
    }
}