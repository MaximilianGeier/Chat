using System.Net;
using System.Text;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BlazorApp1.Pages;

public class Test : PageModel
{
    [BindProperty]
    public string UserName { get; set; }
    public string Message { get; set; }

    private readonly HttpClient _httpClient;

    public Test(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task OnGet()
    {
        Message = "Something";
        
        var url = "http://localhost:1977/user/dad";
        
        // get authentication token from Identity
        var token = HttpContext.User.FindFirst("AuthenticationToken")?.Value;

        // create http client with authentication token
        var client = _httpClient;// new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // create http request with data
        var data = new { MyProperty = "my value" };
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Content = content;
        var cookies = new CookieContainer();
        foreach (var cook in HttpContext.Request.Cookies)
            cookies.Add(new Uri(url), new Cookie(cook.Key, cook.Value));
        if (cookies.Count > 0)
        {
            request.Headers.Add("Cookie", cookies.GetCookieHeader(new Uri(url)));
            var response = await client.SendAsync(request);
            //Message = await response.Content.ReadAsStringAsync();
            var builder = new StringBuilder();
            foreach (var cook in HttpContext.Request.Cookies)
                builder.Append(cook.Key + " : " + cook.Value + "\n");
            Message = "token: " + (await response.Content.ReadAsStringAsync());
        }

        // process http response
        

        /*var result = await _httpClient.GetAsync("http://localhost:1977/user/dad");
        Message = await result.Content.ReadAsStringAsync();*/
    }
    
    /*public async void OnGetAsync()
    {
        UserModel _userModel;
        
        if (UserName == "")
        {
            Message = "Поле пустое!";
            return;
        }
        Message = "Получение данных";
        var response = await _httpClient.GetAsync("http://localhost:1977/user/" + UserName);
        Message = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            _userModel = await response.Content.ReadFromJsonAsync<UserModel>();
            Message = _userModel.Id;
        
        }
        else
        {
            Message = "Не знаю такого!";
        }
        
    }*/
}