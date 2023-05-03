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

    [HttpPost]
    public IActionResult Create([FromBody] CreateUser request)
    {
        IQueryable<User> same = _context.Users.Where(x => x.Login == request.Login);

        if (same.Count() != 0)
        {
            return Conflict();
        }

        var user = new User
        {
            Name = request.Name,
            Login = request.Login
        };
        _context.Users.Add(user);

        _context.SaveChanges();

        return Ok();
    }
}