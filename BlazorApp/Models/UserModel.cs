using System.Text.Json.Serialization;
using Chat.Entities;

namespace Chat.Models;

public class UserModel
{
    public bool IsAuthenticated { get; set; }
    public string Id { get; set; }
    public string UserName { get; set; }
}