using Chat.Entities;
using Microsoft.AspNetCore.Components.Authorization;

namespace Chat.Services;

public class CustomStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService api;
    private ApplicationUser _currentApplicationUser;
    
    public CustomStateProvider(IAuthService api)
    {
        this.api = api;
    }
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }
}