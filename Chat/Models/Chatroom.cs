using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class Chatroom
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
}