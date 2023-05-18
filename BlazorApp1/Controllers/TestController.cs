using BlazorApp1.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Controllers;

public class TestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> TestGet(string userName)
    {
        Console.WriteLine(userName);
        return Ok("FFFFFFFFFFFFFFF");
    }
}