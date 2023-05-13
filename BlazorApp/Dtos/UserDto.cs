using System.Text.Json.Serialization;
using Chat.Models;

namespace Chat.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public UserDto(ApplicationUser applicationUser)
    {
        Name = applicationUser.UserName;
    }
    public UserDto()
    {
    }
}