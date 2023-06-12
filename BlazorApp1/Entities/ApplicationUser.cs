using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BlazorApp1.Entities;

public class ApplicationUser : IdentityUser
{
    [InverseProperty(nameof(Chatroom.Users))]
    public ICollection<Chatroom> Chatrooms { get; set; } = new List<Chatroom>();
    public bool IsDeleted { get; set; }
    [InverseProperty(nameof(Chatroom.Admins))]
    public ICollection<Chatroom> AdminOf { get; set; } = new List<Chatroom>();
}