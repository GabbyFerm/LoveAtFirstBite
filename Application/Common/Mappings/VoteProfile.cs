using Application.Votes.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Common.Mappings
{
    public class VoteProfile : Profile
    {
        public VoteProfile()
        {
            CreateMap<Vote, VoteRecordDto>();
        }
    }
}
