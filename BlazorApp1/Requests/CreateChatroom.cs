namespace BlazorApp1.Requests;

public class CreateChatroom
{
    public IEnumerable<String> UserNames { get; set; }
    public string Title { get; set; }
}