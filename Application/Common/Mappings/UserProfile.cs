using Application.Authorize.Commands.Register;
using AutoMapper;
using Domain.Models;

namespace Application.Common.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterCommand, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // we hash it manually
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserEmail.ToLower())); // normalize email
        }
    }
}
