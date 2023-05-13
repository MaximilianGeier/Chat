using AutoMapper;
using Chat.Entities;
using Chat.Models;

namespace Chat.Mappers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<ApplicationUser, UserModel>();
        CreateMap<Chatroom, ChatroomModel>();
        CreateMap<ChatMessage, ChatMessageModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName));
    }
}