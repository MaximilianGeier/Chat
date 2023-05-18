using System.Text;
using System.Text.Json;
using AutoMapper;
using BlazorApp1.Data;
using BlazorApp1.Entities;
using BlazorApp1.Hubs;
using BlazorApp1.Models;
using BlazorApp1.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp1.Controllers;

[Route("/chat/message")]
[ApiController]
public class MessageController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub> _hub;

    public MessageController(ApplicationDbContext context, IMapper mapper, IHubContext<ChatHub> hub)
    {
        _context = context;
        _mapper = mapper;
        _hub = hub;
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] long id)
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
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMessage request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => User.Identity.Name == user.UserName);
        if (user is null)
            return NotFound("Authorized user not found");
        var chat = await _context.Chatrooms.FirstOrDefaultAsync(x => request.ChatId == x.Id);
        if (chat is null)
            return NotFound("Chatroom not found");
        
        var message = new ChatMessage()
        {
            Text = request.Text,
            ApplicationUser = user,
            Chatroom = chat,
            CreationDate = DateTime.Now,
            UpdateTime = DateTime.Now
        };
        _context.Add(message);
        var c = await _context.SaveChangesAsync();
        ChatMessageModel chatMessageModel = _mapper.Map<ChatMessageModel>(message);

        var serializeMsg = JsonSerializer.Serialize(chatMessageModel);
        Console.WriteLine("(((((((((((((((");
        Console.WriteLine(serializeMsg);
        await _hub.Clients.Groups("group1").SendAsync("ReceiveNewMessage", chatMessageModel);

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