using Chat.Database;
using Chat.Dtos;
using Chat.Models;
using Chat.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Controllers;

[Route("/chat/message")]
public class MessageController : Controller
{
    private readonly ChatContext _context;

    public MessageController(ChatContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id:int}")]
    public IActionResult Get([FromRoute] int id)
    {
        var message = _context.ChatMessages.FirstOrDefault(x => x.Id == id);
        if (message is null)
            return NotFound();

        return Ok(new ChatMessageDto(message));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateMessage request)
    {
        var user = _context.Users.FirstOrDefault(x => request.UserId == x.Id);
        if (user is null)
            return NotFound();
        var chat = _context.Chatrooms.FirstOrDefault(x => request.ChatId == x.Id);
        if (chat is null)
            return NotFound();
        
        var message = new ChatMessage()
        {
            Text = request.Text,
            User = user,
            Chatroom = chat,
            CreationDate = DateTime.Now,
            UpdateTime = DateTime.Now
        };
        _context.Add(message);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPatch("{id:int}")]
    public IActionResult Update([FromBody] UpdateMessage request, [FromRoute] int id)
    {
        var message = _context.ChatMessages.FirstOrDefault(x => x.Id == id);
        if (message is null)
            return NotFound();

        message.Text = request.Text;

        _context.SaveChanges();
        
        return Ok();
    }
}