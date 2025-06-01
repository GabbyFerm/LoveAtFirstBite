using Application.Rounds.DTOs;
using AutoMapper;
using Domain.Models;

namespace Application.Common.Mappings
{
    public class RoundProfile : Profile
    {
        public RoundProfile()
        {
            CreateMap<Round, RoundDto>();
        }
    }
}
