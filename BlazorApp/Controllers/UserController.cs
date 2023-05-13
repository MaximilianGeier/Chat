using AutoMapper;
using Chat.Database;
using Chat.Models;
using Chat.Entities;
using Chat.Hubs;
using Chat.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Controllers;

[Route("/user")]
[ApiController]
public class UserController : Controller
{
    private readonly ChatContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub> _hub;
    
    public UserController(ChatContext context, IMapper mapper, IHubContext<ChatHub> hub)
    {
        _context = context;
        _mapper = mapper;
        _hub = hub;
    }
    
    [HttpGet("{username}")]
    public async Task<IActionResult> GetAsync([FromRoute] string userName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null)
            return NotFound();
        if (user.IsDeleted)
            return NotFound();
        
        //await _hub.Clients.All.SendAsync("ReceiveMessage", "send some data");
        _hub.Clients.Groups("group2").SendAsync("ReceiveMessage", "send some data");

        return Ok(_mapper.Map<UserModel>(user));
    }

    [HttpPatch("{username}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateUser request, [FromRoute] string userName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null)
            return NotFound();

        user.UserName = request.Name;

        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteAsync([FromBody] UpdateMessage request, [FromRoute] string userName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null)
            return NotFound();

        user.IsDeleted = true;

        await _context.SaveChangesAsync();

        return Ok();
    }
}