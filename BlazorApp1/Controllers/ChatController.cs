using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using BlazorApp1.Data;
using BlazorApp1.Models;
using BlazorApp1.Entities;
using BlazorApp1.Hubs;
using BlazorApp1.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Controllers;

[Route("/chat")]
[ApiController]
public class ChatController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub> _hub;

    public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
        IMapper mapper, IHubContext<ChatHub> hub)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _hub = hub;
    }

    [HttpGet("{id:int}")]
    [Authorize]
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

        return Ok(_mapper.Map<ChatroomModel>(chatroom));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromBody] CreateChatroom request)
    {
        if (!request.UserNames.All(userName => _context.Users.Select(user => user.UserName).Contains(userName)))
            return NotFound("Пользователь не найден");

        var users = new List<ApplicationUser>();
        if (request.UserNames.Any())
            users.AddRange(await _context.Users.Where(user => request.UserNames.Contains(user.UserName)).ToListAsync());
        users.Add(await _context.Users.Where(user => user.UserName == User.Identity.Name).FirstOrDefaultAsync());
        if (users.Count == 0)
            return NotFound();
        
        Console.WriteLine($"{request.Title} \n {users}");
        var chatroom = new Chatroom
        {
            Title = request.Title,
            Users = users,
            Messages = new List<ChatMessage>()
        };
        
        _context.Chatrooms.Add(chatroom);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch("{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateAsync([FromBody] CreateChatroom request, [FromRoute] int id)
    {
        var chatroom = await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == id);
        if (chatroom is null)
            return NotFound();

        chatroom.Title = request.Title;

        await _context.SaveChangesAsync();

        var chatrooms = await _context.Chatrooms
            .Include(x => x.Users)
            .Where(x => !x.IsDeleted && x.Users.Any(u => u.UserName == User.Identity.Name))
            .ToListAsync();
        List<ChatroomModel> chatroomsModels = 
            chatrooms.Select(chatroom => _mapper.Map<ChatroomModel>(chatroom)).ToList();

        await _hub.Clients.Groups("group1").SendAsync("ReceiveChatrooms", chatroomsModels);
        
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var chatroom = await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == id);
        if (chatroom is null)
            return NotFound();

        chatroom.IsDeleted = true;

        await _context.SaveChangesAsync();
        
        var chatrooms = await _context.Chatrooms
            .Include(x => x.Users)
            .Where(x => !x.IsDeleted && x.Users.Any(u => u.UserName == User.Identity.Name))
            .ToListAsync();
        List<ChatroomModel> chatroomsModels = 
            chatrooms.Select(chatroom => _mapper.Map<ChatroomModel>(chatroom)).ToList();

        await _hub.Clients.Groups("group1").SendAsync("ReceiveChatrooms", chatroomsModels);

        return Ok();
    }
    
    [HttpGet("{id:int}/users")]
    [Authorize]
    public async Task<IActionResult> GetUsersAsync([FromRoute] int id)
    {
        var users = await _context.Users
            .Where(user => user.Chatrooms.Any(item => item.Id == id) && !user.IsDeleted).ToListAsync();
        if (users.Count == 0)
            return NotFound();

        return Ok(_mapper.Map<List<UserModel>>(users));
    }

    [HttpPost("user")]
    [Authorize]
    public async Task<IActionResult> AddUserAsync([FromBody] AddUser request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(user => user.UserName == request.UserName && !user.IsDeleted);
        
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
    [Authorize]
    public async Task<IActionResult> GetMessagesAsync([FromRoute] int id)
    {
        var chatroom = _context.Chatrooms.FirstOrDefault(chatroom => chatroom.Id == id);
        if (chatroom is null)
            return NotFound();
        if (chatroom is { IsDeleted: true })
            return NotFound();
        
        var messages = await  _context.ChatMessages
            .Include(x => x.ApplicationUser)
            .Where(message => message.Chatroom.Id == id && !message.IsDeleted).ToListAsync();

        return Ok(messages.Select(message => _mapper.Map<ChatMessageModel>(message)));
    }
    
    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllAsync()
    {
        var chatrooms = await _context.Chatrooms
            .Include(x => x.Users)
            .Where(x => !x.IsDeleted && x.Users.Any(u => u.UserName == User.Identity.Name))
            .ToListAsync();
        if (chatrooms.Count == 0)
            return NotFound();
        return Ok(chatrooms.Select(chatroom => _mapper.Map<ChatroomModel>(chatroom)));
    }
    
    [HttpDelete("user")]
    [Authorize]
    public async Task<IActionResult> DeleteUserAsync([FromBody] DeleteUserFromChat request)
    {
        var chatroom = await _context.Chatrooms
            .Include(ct => ct.Users)
            .FirstOrDefaultAsync(chatroom => chatroom.Id == request.ChatId && !chatroom.IsDeleted);
        if (chatroom == null)
            return NotFound("Чат не найден!");

        var user = chatroom.Users
            .FirstOrDefault(user => user.UserName == request.UserName);
        if (user == null)
            return NotFound("Пользователь не найден!");
        
        if (!chatroom.Users.Any(u => u.UserName == User.Identity.Name))
            return new ObjectResult( User.Identity.Name + " Вы не состоите в чате!") { StatusCode = 403 };

        chatroom.Users.Remove(user);
        user.Chatrooms.Remove(chatroom);

        await _context.SaveChangesAsync();

        return Ok();
    }
}