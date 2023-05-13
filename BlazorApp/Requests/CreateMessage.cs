namespace Chat.Requests;

public class CreateMessage
{
    public int UserId { get; set; }
    public int ChatId { get; set; }
    public string Text { get; set; }
}