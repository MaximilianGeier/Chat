using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Controllers;

[Route("test")]
[ApiController]
public class test : Controller
{
    // GET
    public IActionResult Index()
    {
        return Ok("kekoekfo");
    }
}