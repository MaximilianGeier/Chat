using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Chat.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string Login { get; set; }
    public ICollection<Chatroom> Chatrooms { get; set; } = new List<Chatroom>();
    public bool IsDeleted { get; set; }
}