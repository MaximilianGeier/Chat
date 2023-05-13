using AutoMapper;
using Chat.Database;
using Chat.Models;
using Chat.Entities;
using Chat.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Controllers;

[Route("/chat/message")]
[ApiController]
public class MessageController : Controller
{
    private readonly ChatContext _context;
    private readonly IMapper _mapper;

    public MessageController(ChatContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
    {
        var message = await _context.ChatMessages
            .Include(x => x.ApplicationUser)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (message is null)
            return NotFound();
        if (message.IsDeleted)
            return NotFound();

        return Ok(_mapper.Map<ChatMessageModel>(message));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMessage request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => request.UserName == user.UserName);
        if (user is null)
            return NotFound();
        var chat = await _context.Chatrooms.FirstOrDefaultAsync(x => request.ChatId == x.Id);
        if (chat is null)
            return NotFound();
        
        var message = new ChatMessage()
        {
            Text = request.Text,
            ApplicationUser = user,
            Chatroom = chat,
            CreationDate = DateTime.Now,
            UpdateTime = DateTime.Now
        };
        _context.Add(message);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateMessage request, [FromRoute] int id)
    {
        var message = await _context.ChatMessages.FirstOrDefaultAsync(x => x.Id == id);
        if (message is null)
            return NotFound();

        message.Text = request.Text;
        message.IsUpdated = true;
        message.UpdateTime = DateTime.Now;

        await _context.SaveChangesAsync();
        
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromBody] UpdateMessage request, [FromRoute] int id)
    {
        var message = await _context.ChatMessages.FirstOrDefaultAsync(x => x.Id == id);
        if (message is null)
            return NotFound();

        message.IsDeleted = true;

        await _context.SaveChangesAsync();

        return Ok();
    }
}