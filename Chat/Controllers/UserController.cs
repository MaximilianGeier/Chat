using Chat.Database;
using Chat.Dtos;
using Chat.Models;
using Chat.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Controllers;

[Route("/user")]
public class UserController : Controller
{
    private readonly ChatContext _context;

    public UserController(ChatContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id:int}")]
    public IActionResult Get([FromRoute] int id)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == id);
        if (user is null)
            return NotFound();
        if (user.IsDeleted)
            return NotFound();

        return Ok(new UserDto(user));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateUser request)
    {
        IQueryable<User> same = _context.Users.Where(x => x.Login == request.Login);

        if (same.Count() != 0)
            return Conflict();

        var user = new User
        {
            Name = request.Name,
            Login = request.Login
        };
        _context.Users.Add(user);

        _context.SaveChanges();

        return Ok();
    }
    
    [HttpPatch("{id:int}")]
    public IActionResult Update([FromBody] UpdateUser request, [FromRoute] int id)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == id);
        if (user is null)
            return NotFound();

        user.Name = request.Name;

        _context.SaveChanges();
        
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromBody] UpdateMessage request, [FromRoute] int id)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == id);
        if (user is null)
            return NotFound();

        user.IsDeleted = true;

        _context.SaveChanges();

        return Ok();
    }
}