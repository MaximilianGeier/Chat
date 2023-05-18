namespace BlazorApp1.Models;

public class UserModel
{
    public bool IsAuthenticated { get; set; }
    public string Id { get; set; }
    public string UserName { get; set; }
    public Dictionary<string, string> Claims { get; set; }
}