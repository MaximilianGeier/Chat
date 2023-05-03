using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Login { get; set; }
    public ICollection<Chatroom> Chatrooms { get; set; } = new List<Chatroom>();
    public bool IsDeleted { get; set; }
}