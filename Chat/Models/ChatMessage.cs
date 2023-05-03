using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Chat.Models;

public class ChatMessage
{
    public long Id { get; set; }
    [Required]
    public string Text { get; set; }
    [Required]
    public User User { get; set; }
    [Required]
    public Chatroom Chatroom { get; set; }
    [Required]
    public DateTime CreationDate { get; set; }
    [Required] //[DefaultValue(DefaultValueSql: "GetDate()")]
    public DateTime UpdateTime { get; set; }
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
    [DefaultValue(false)]
    public bool IsUpdated { get; set; }
}