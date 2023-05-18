using Microsoft.AspNetCore.SignalR;

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
        Groups.AddToGroupAsync(Context.ConnectionId, "group1");
        
        Thread.Sleep(2000);
        await Clients.All.SendAsync("ReceiveMessage", "Recived date");
        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
    }

    public static string GetData()
    {
        var r = new Random();
        return r.Next().ToString();
    }
}