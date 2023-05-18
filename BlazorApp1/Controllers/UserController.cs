using AutoMapper;
using BlazorApp1.Data;
using BlazorApp1.Models;
using BlazorApp1.Hubs;
using BlazorApp1.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Controllers;

[Route("/user")]
[ApiController]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub> _hub;
    
    public UserController(ApplicationDbContext context, IMapper mapper, IHubContext<ChatHub> hub)
    {
        _context = context;
        _mapper = mapper;
        _hub = hub;
    }
    
    [HttpGet("{username}")]
    [Authorize]
    public async Task<IActionResult> GetAsync([FromRoute] string userName)
    {
        Console.WriteLine(User.Identity.IsAuthenticated);
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null)
            return NotFound();
        if (user.IsDeleted)
            return NotFound();

        return Ok(_mapper.Map<UserModel>(user));
    }

    [HttpPatch("{username}")]
    [Authorize]
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
    [Authorize]
    public async Task<IActionResult> DeleteAsync([FromRoute] string userName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null)
            return NotFound();

        user.IsDeleted = true;

        await _context.SaveChangesAsync();

        return Ok();
    }
}