namespace Chat.Models;

public class Chatroom
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}