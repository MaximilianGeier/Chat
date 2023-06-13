namespace BlazorApp1.Models;

public class ChatMessageModel
{
    public long Id { get; set; }
    public string Text { get; set; }
    public string UserName { get; set; }
    public DateTime Date { get; set; }
    public bool IsUpdated { get; set; }
    public long ChatroomId { get; set; }
}