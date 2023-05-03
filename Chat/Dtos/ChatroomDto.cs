using Chat.Models;

namespace Chat.Dtos;

public class ChatroomDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<UserDto> Users { get; set; }
    public IEnumerable<ChatMessageDto> Messages { get; set; }

    public ChatroomDto(Chatroom chatroom)
    {
        Id = chatroom.Id;
        Title = chatroom.Title;
        Users = chatroom.Users.Select(x => new UserDto(x));
        Messages = chatroom.Messages.Select(x => new ChatMessageDto(x));
    }
}