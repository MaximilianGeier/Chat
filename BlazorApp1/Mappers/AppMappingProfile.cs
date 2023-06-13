using AutoMapper;
using BlazorApp1.Entities;
using BlazorApp1.Models;

namespace BlazorApp1.Mappers;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<ApplicationUser, UserModel>();
        CreateMap<Chatroom, ChatroomModel>();
        CreateMap<ChatMessage, ChatMessageModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
            .ForMember(dest => dest.ChatroomId, opt => opt.MapFrom(src => src.Chatroom.Id));
    }
}