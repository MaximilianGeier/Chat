using Chat.Database;
using Chat.Dtos;
using Chat.Models;
using Chat.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Controllers;

[Route("/chat")]
[ApiController]
public class ChatController : Controller
{
    private readonly ChatContext _context;

    public ChatController(ChatContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
    {
        var chatroom = await _context.Chatrooms
            .Include(x => x.Users.Where(y => !y.IsDeleted))
            .Include(x => x.Messages.Where(y => !y.IsDeleted))
            .FirstOrDefaultAsync(x => x.Id == id);
        if (chatroom is null)
            return NotFound();
        if (chatroom.IsDeleted)
            return NotFound();

        var dto = new ChatroomDto(chatroom);

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateChatroom request)
    {
        if (!_context.Users.All(x => request.Users.Contains(x.Id)))
            return NotFound();

        var users = await _context.Users.Where(x => request.Users.Contains(x.Id)).ToListAsync();
        if (users.Count == 0)
            return NotFound();
        
        var chatroom = new Chatroom
        {
            Title = request.Title,
            Users = users
        };
        
        _context.Add(chatroom);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromBody] CreateChatroom request, [FromRoute] int id)
    {
        var chatroom = await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == id);
        if (chatroom is null)
            return NotFound();

        chatroom.Title = request.Title;

        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromBody] UpdateMessage request, [FromRoute] int id)
    {
        var chatroom = await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == id);
        if (chatroom is null)
            return NotFound();

        chatroom.IsDeleted = true;

        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet("{id:int}/users")]
    public async Task<IActionResult> GetUsersAsync([FromRoute] int id)
    {
        var users = await _context.Users
            .Where(user => user.Chatrooms.Any(item => item.Id == id) && !user.IsDeleted).ToListAsync();
        if (users.Count == 0)
            return NotFound();

        var usersDto = new UserDto[users.Count()];

        return Ok(users);
    }

    [HttpPatch("user")]
    public async Task<IActionResult> AddUserAsync([FromBody] AddUser request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(user => user.Id == request.UserId && !user.IsDeleted);
        
        var chatroom = await _context.Chatrooms
            .FirstOrDefaultAsync(item => item.Id == request.ChatroomId && !item.IsDeleted);
        
        if (user is null || chatroom is null || chatroom.Users.Contains(user))
            return Conflict();
        
        chatroom.Users.Add(user);
        
        _context.Update(chatroom);
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet("messages/{id:int}")]
    public async Task<IActionResult> GetMessagesAsync([FromRoute] int id)
    {
        var chatroom = _context.Chatrooms.FirstOrDefault(chatroom => chatroom.Id == id);
        if (chatroom is null)
            return NotFound();
        if (chatroom is { IsDeleted: true })
            return NotFound();
        
        var messages = await  _context.ChatMessages
            .Include(x => x.User)
            .Where(message => message.Chatroom.Id == id && !message.IsDeleted).ToListAsync();

        return Ok(messages.Select(x => new ChatMessageDto(x)));
    }
}