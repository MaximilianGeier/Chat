using Chat.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database;

public class ChatContext : DbContext
{
    public DbSet<Chatroom> Chatrooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    public ChatContext(DbContextOptions<ChatContext> options) : base(options)
    {
        
    }
}