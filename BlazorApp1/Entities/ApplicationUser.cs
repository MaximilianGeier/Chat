using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Chatroom> Chatrooms { get; set; } = new List<Chatroom>();
    public bool IsDeleted { get; set; }
}