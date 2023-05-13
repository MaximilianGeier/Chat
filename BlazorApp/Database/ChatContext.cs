using Chat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database;

public class ChatContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Chatroom> Chatrooms { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    public ChatContext(DbContextOptions<ChatContext> options) : base(options)
    {
        
    }
}