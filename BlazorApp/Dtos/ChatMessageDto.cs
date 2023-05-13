using Chat.Models;

namespace Chat.Dtos;

public class ChatMessageDto
{
    public long Id { get; set; }
    public string Text { get; set; }
    public string UserName { get; set; }
    public DateTime Date { get; set; }
    public bool IsUpdated { get; set; }

    public ChatMessageDto(ChatMessage chatMessage)
    {
        Id = chatMessage.Id;
        Text = chatMessage.Text;
        UserName = chatMessage.ApplicationUser.UserName;
        Date = chatMessage.CreationDate;
        IsUpdated = chatMessage.IsUpdated;
    }
}