using AutoMapper;
using Chat.Database;
using Chat.Models;
using Chat.Entities;
using Chat.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Controllers;

[Route("/chat")]
[ApiController]
public class ChatController : Controller
{
    private readonly ChatContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public ChatController(ChatContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
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
    public async Task<IActionResult> CreateAsync([FromBody] CreateChatroom request)
    {
        if (User.Identity.IsAuthenticated)
            Console.WriteLine("AUTHENTED!!!!");
        else
            Console.WriteLine("NOT AUTHENTED!!!!");
        
        if (!request.UserNames.All(userName => _context.Users.Select(user => user.UserName).Contains(userName)))
            return NotFound();

        var users = new List<ApplicationUser>();
        if (request.UserNames.Any())
            users.AddRange(await _context.Users.Where(user => request.UserNames.Contains(user.UserName)).ToListAsync());
        users.Add(await _context.Users.Where(user => user.UserName == User.Identity.Name).FirstOrDefaultAsync());
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

        var usersDto = new UserModel[users.Count()];

        return Ok(users);
    }

    [HttpPatch("user")]
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
}