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
            .Include(x => x.Users)
            .FirstOrDefault(x => x.Id == id);

        if (chatroom is null)
        {
            return NotFound();
        }

        var dto = new ChatroomDto
        {
            Id = chatroom.Id,
        };

        return Ok(dto);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateChatroom request)
    {
        bool all = _context.Users.All(x => request.Users.Contains(x.Id));

        if (!all)
        {
            return NotFound();
        }
        var chatroom = new Chatroom
        {
            Title = request.Title
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
        {
            return NotFound();
        }

        chatroom.Title = request.Title;

        _context.SaveChanges();

        return Ok();
    }
    
    [Route("/users"), HttpGet("{id:int}")]
    public IActionResult GetUsers([FromRoute] int id)
    {
        var users = _context.Chatrooms
            .Include(x => x.Users)
            .Where(x => x.Id == id);

        if (chatroom is null)
        {
            return NotFound();
        }

        var dto = new ChatroomDto
        {
            Id = chatroom.Id,
/*                Users = chatroom.Users.Select(x => new UserDto
                {
                    Id = 1,
                    Name = "1"
                })*/
        };

        return Ok(dto);
    }
}