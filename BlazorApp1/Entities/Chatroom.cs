using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Entities;

public class Chatroom
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
}