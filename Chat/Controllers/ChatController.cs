using Chat.Database;
using Chat.Dtos;
using Chat.Models;
using Chat.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Controllers;

[Route("/chat")]
public class ChatController : Controller
{
    private readonly ChatContext _context;

    public ChatController(ChatContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}")]
    public IActionResult Get([FromRoute] int id)
    {
        var chatroom = _context.Chatrooms
            .Include(x => x.Users.Where(y => !y.IsDeleted))
            .Include(x => x.Messages.Where(y => !y.IsDeleted))
            .FirstOrDefault(x => x.Id == id);
        if (chatroom is null)
            return NotFound();
        if (chatroom.IsDeleted)
            return NotFound();

        var dto = new ChatroomDto(chatroom);

        return Ok(dto);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateChatroom request)
    {
        if (!_context.Users.All(x => request.Users.Contains(x.Id)))
            return NotFound();

        var users = _context.Users.Where(x => request.Users.Contains(x.Id)).ToList();
        if (users.Count == 0)
            return NotFound();
        
        var chatroom = new Chatroom
        {
            Title = request.Title,
            Users = users
        };
        
        _context.Add(chatroom);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPatch("{id:int}")]
    public IActionResult Update([FromBody] CreateChatroom request, [FromRoute] int id)
    {
        var chatroom = _context.Chatrooms.FirstOrDefault(x => x.Id == id);
        if (chatroom is null)
            return NotFound();

        chatroom.Title = request.Title;

        _context.SaveChanges();

        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromBody] UpdateMessage request, [FromRoute] int id)
    {
        var chatroom = _context.Chatrooms.FirstOrDefault(x => x.Id == id);
        if (chatroom is null)
            return NotFound();

        chatroom.IsDeleted = true;

        _context.SaveChanges();

        return Ok();
    }
    
    [HttpGet("{id:int}/users")]
    public IActionResult GetUsers([FromRoute] int id)
    {
        var users = _context.Users
            .Where(user => user.Chatrooms.Any(item => item.Id == id) && !user.IsDeleted).ToList();
        if (users.Count == 0)
            return NotFound();

        var usersDto = new UserDto[users.Count()];

        return Ok(users);
    }
}