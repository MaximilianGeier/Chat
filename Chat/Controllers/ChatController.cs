using Chat.Database;
using Chat.Dtos;
using Chat.Models;
using Chat.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat.Controllers;

[Route("/chat")]
public class ChatController : Controller
{
    private readonly ChatContext _context;

    public ChatController(ChatContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}")]
    public IActionResult Get([FromRoute] int id)
    {
<<<<<<< Updated upstream
        var chatroom = _context.Chatroom
=======
        var chatroom = _context.Chatrooms
>>>>>>> Stashed changes
            .Include(x => x.Users)
            .FirstOrDefault(x => x.Id == id);

        if (chatroom is null)
        {
            return NotFound();
        }

        var dto = new ChatroomDto
        {
            Id = chatroom.Id,
<<<<<<< Updated upstream
/*                Users = chatroom.Users.Select(x => new UserDto
                {
                    Id = 1,
                    Name = "1"
                })*/
=======
/*				Users = chatroom.Users.Select(x => new UserDto
				{
					Id = 1,
					Name = "1"
				})*/
>>>>>>> Stashed changes
        };

        return Ok(dto);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateChatroom request)
    {
        bool all = _context.Users.All(x => request.Users.Contains(x.Id));

        if (!all)
        {
            return NotFound();
        }
        var chatroom = new Chatroom
        {
            Title = request.Title
        };
        _context.Add(chatroom);

        _context.SaveChanges();

        return Ok();
    }

    [HttpPatch("{id:int}")]
    public IActionResult Update([FromBody] CreateChatroom request, [FromRoute] int id)
    {
<<<<<<< Updated upstream
        var chatroom = _context.Chatroom.FirstOrDefault(x => x.Id == id);
=======
        var chatroom = _context.Chatrooms.FirstOrDefault(x => x.Id == id);
>>>>>>> Stashed changes

        if (chatroom is null)
        {
            return NotFound();
        }

        chatroom.Title = request.Title;

        _context.SaveChanges();

        return Ok();
    }
    
    [Route("/users"), HttpGet("{id:int}")]
    public IActionResult GetUsers([FromRoute] int id)
    {
        var users = _context.Chatroom
            .Include(x => x.Users)
            .Where(x => x.Id == id);

        if (chatroom is null)
        {
            return NotFound();
        }

        var dto = new ChatroomDto
        {
            Id = chatroom.Id,
/*                Users = chatroom.Users.Select(x => new UserDto
                {
                    Id = 1,
                    Name = "1"
                })*/
        };

        return Ok(dto);
    }
}