using Application.Votes.Dtos;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
