using Chat.Models;
using Chat.Requests;

namespace Chat.Services;

public interface IAuthService
{
    Task Login(LoginRequest loginRequest);
    Task Register(RegisterRequest registerRequest);
    Task Logout();
    //Task<User> CurrentUserInfo();
}