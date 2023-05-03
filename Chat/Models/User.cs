namespace Chat.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
<<<<<<< Updated upstream
    public ICollection<Chatroom> Chatrooms { get; set; } = new List<Chatroom>();
=======
    public ICollection<Chatroom> Chats { get; set; }
>>>>>>> Stashed changes
}