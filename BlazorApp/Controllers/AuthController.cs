using Chat.Entities;
using Chat.Models;
using Chat.Requests;
using Chat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[Route("/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        Console.WriteLine("LOGIN!!!!!!!!!!!");
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null) 
            return BadRequest("User does not exist");
        var singInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!singInResult.Succeeded) 
            return BadRequest("Invalid password");
        await _signInManager.SignInAsync(user, request.RememberMe);
        Console.WriteLine("LOGIN2!!!!!!!!!!!");
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest parameters)
    {
        var user = new ApplicationUser();
        user.UserName = parameters.UserName;
        user.Email = parameters.Email;
        var result = await _userManager.CreateAsync(user, parameters.Password);
        if (!result.Succeeded) 
            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        
        return await Login(new LoginRequest
        {
            UserName = parameters.UserName,
            Password = parameters.Password
        });
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
    
    [HttpGet("currentuser")]
    public UserModel CurrentUserInfo()
    {
        return new UserModel
        {
            IsAuthenticated = User.Identity.IsAuthenticated,
            UserName = User.Identity.Name,
        };
    }
}