using Chat.Database;
using Chat.Dtos;
using Chat.Models;
using Chat.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Controllers;

/*[Route("/user")]
[ApiController]
public class UserController : Controller
{
    private readonly ChatContext _context;

    public UserController(ChatContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null)
            return NotFound();
        if (user.IsDeleted)
            return NotFound();

        return Ok(new UserDto(user));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUser request)
    {
        var same = await _context.Users
            .Where(x => x.Login == request.Login)
            .ToArrayAsync();

        if (same.Count() != 0)
            return Conflict();

        var user = new User
        {
            Name = request.Name,
            Login = request.Login
        };
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok(User.Identity.IsAuthenticated.ToString());
    }
    
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateUser request, [FromRoute] int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null)
            return NotFound();

        user.Name = request.Name;

        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromBody] UpdateMessage request, [FromRoute] int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null)
            return NotFound();

        user.IsDeleted = true;

        await _context.SaveChangesAsync();

        return Ok();
    }
}*/