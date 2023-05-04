namespace Chat.Requests;

public class CreateChatroom
{
    public IEnumerable<int> Users { get; set; }
    public string Title { get; set; }
}