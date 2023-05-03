using Chat.Models;

namespace Chat.Dtos;

public class ChatroomDto
{
    public int Id { get; set; }
    public IEnumerable<UserDto> Users { get; set; }
    public IEnumerable<ChatMessage> Messages { get; set; }
}