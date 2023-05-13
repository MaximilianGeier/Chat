namespace Chat.Requests;

public class CreateChatroom
{
    public IEnumerable<String> UserNames { get; set; }
    public string Title { get; set; }
}