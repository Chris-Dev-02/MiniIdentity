using AutoMapper;
using MiniIdentity.Application.DTOs.Responses;
using MiniIdentity.Enterprise.Models;

namespace MiniIdentity.Application.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}
