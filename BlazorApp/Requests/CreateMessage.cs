namespace Chat.Requests;

public class CreateMessage
{
    public string UserName { get; set; }
    public int ChatId { get; set; }
    public string Text { get; set; }
}