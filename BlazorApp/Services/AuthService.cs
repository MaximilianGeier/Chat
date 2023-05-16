using Chat.Entities;
using Chat.Models;
using Chat.Requests;

namespace Chat.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<UserModel> CurrentUserInfo()
    {
        var result = await _httpClient.GetFromJsonAsync<UserModel>("auth/currentuser");
        return result;
    }
    
    public async Task Login(LoginRequest loginRequest)
    {
        var result = await _httpClient.PostAsJsonAsync("auth/login", loginRequest);
        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) 
            throw new Exception(await result.Content.ReadAsStringAsync());
        result.EnsureSuccessStatusCode();
    }
    
    public async Task Logout()
    {
        var result = await _httpClient.PostAsync("auth/logout", null);
        result.EnsureSuccessStatusCode();
    }
    
    public async Task Register(RegisterRequest registerRequest)
    {
        var result = await _httpClient.PostAsJsonAsync("auth/register", registerRequest);
        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) 
            throw new Exception(await result.Content.ReadAsStringAsync());
        result.EnsureSuccessStatusCode();
    }
}