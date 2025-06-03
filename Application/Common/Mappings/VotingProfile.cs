using Application.Votes.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Commen.Mappings
{
    public class VotingProfile : Profile
    {
        public VotingProfile() 
        {
         CreateMap<Vote, VoteDto>().ReverseMap();
        }
    }
}
