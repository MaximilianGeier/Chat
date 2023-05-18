using System.Net;
using System.Text;
using BlazorApp1.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Org.BouncyCastle.Asn1.Ocsp;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Chat.Services;

public class AuthRequestService
{
    private IHttpContextAccessor _httpContextAccessor;
    private HttpClient _httpClient;
    
    public AuthRequestService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClient;
    }
    
    public async Task<R> MakeAuthorizedPost<T, R>(string url, T data)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        AddAuthCookiesToRequest(request, url);
        
        if (_httpContextAccessor.HttpContext.Request.Cookies.Select(pair => pair.Key).Any(key => key.Contains(".AspNetCore.Antiforgery")) &&
            _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(".AspNetCore.Identity.Application"))
        {
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<R>();
        }

        throw new Exception("Not authorized!");
    }
    
    public async Task<HttpResponseMessage?> MakeAuthorizedPost<T>(string url, T data)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        AddAuthCookiesToRequest(request, url);
        
        if (_httpContextAccessor.HttpContext.Request.Cookies.Select(pair => pair.Key).Any(key => key.Contains(".AspNetCore.Antiforgery")) &&
            _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(".AspNetCore.Identity.Application"))
        {
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        throw new Exception("Not authorized!");
    }
    
    public async Task<HttpResponseMessage?> MakeAuthorizedGet(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddAuthCookiesToRequest(request, url);
        
        if (_httpContextAccessor.HttpContext.Request.Cookies.Select(pair => pair.Key).Any(key => key.Contains(".AspNetCore.Antiforgery")) &&
            _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(".AspNetCore.Identity.Application"))
        {
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        throw new Exception("Not authorized!");
    }
    
    public async Task<HttpResponseMessage?> MakeAuthorizedDelete(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        AddAuthCookiesToRequest(request, url);
        
        if (_httpContextAccessor.HttpContext.Request.Cookies.Select(pair => pair.Key).Any(key => key.Contains(".AspNetCore.Antiforgery")) &&
            _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(".AspNetCore.Identity.Application"))
        {
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        throw new Exception("Not authorized!");
    }
    
    public async Task<HttpResponseMessage?> MakeAuthorizedDelete<T>(string url, T data)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        AddAuthCookiesToRequest(request, url);
        
        if (_httpContextAccessor.HttpContext.Request.Cookies.Select(pair => pair.Key).Any(key => key.Contains(".AspNetCore.Antiforgery")) &&
            _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(".AspNetCore.Identity.Application"))
        {
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        throw new Exception("Not authorized!");
    }

    private void AddAuthCookiesToRequest(HttpRequestMessage request, string url)
    {
        var cookies = new CookieContainer();
        foreach (var cook in _httpContextAccessor.HttpContext.Request.Cookies)
            cookies.Add(new Uri(url), new Cookie(cook.Key, cook.Value));
        request.Headers.Add("Cookie", cookies.GetCookieHeader(new Uri(url)));
    }
}