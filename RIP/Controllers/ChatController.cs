using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Controllers;

[ApiController]
[Microsoft.AspNetCore.Components.Route("/chat")]
public class ChatController : Controller
{

    public ChatController()
    {
    }

    [HttpGet("{id:int}")]
    public  IActionResult Get([FromRoute] int id)
    {
        return Ok("rkforrfrfrkf");
    }
}