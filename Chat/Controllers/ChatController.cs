using Chat.Database;
using Chat.Requests;
using Microsoft.AspNetCore.Mvc;

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
        return Ok();
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] CreateChatroom request)
    {
        return Ok();
    }

    [HttpPatch("{id:int}")]
    public IActionResult Update([FromBody] CreateChatroom request, [FromRoute] int id)
    {
        return Ok();
    }
}